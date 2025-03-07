const { PrismaClient } = require('@prisma/client');
const prisma = new PrismaClient();
const { createResponse } = require('../Middleware/CreateResponse');
const jwt = require('jsonwebtoken');
const artController = {


    // Get all art pieces
    async getAllArt(req, res) {
        try {
            const allArt = await prisma.art.findMany();
            res.json(allArt);
        } catch (error) {
            res.status(500).json(createResponse('Error', 'Failed to get all art pieces'));
        }
    },

    // Get all art pieces for a specific user
    async getAllArtWithUserId(req, res) {
        const userId = parseInt(req.params.userId);
        try {
            const userArt = await prisma.art.findMany({
                where: {
                    UserId: userId
                },
                include: {
                    User: {
                        select: {
                            Username: true,
                            Email: true
                        }
                    }
                }
            });
            if (userArt.length === 0) {
                return res.status(404).json(createResponse('Error', 'No art was found'));
            }
            res.json(userArt);
        } catch (error) {
            res.status(500).json(createResponse('Error', 'Failed to retrive user art'));
        }
    },

    // Create a new art piece
    async createArt(req, res) {
        const { Image, Description, Price } = req.body;
        const token = req.headers.authorization?.split(' ')[1];

        if (!token) {
            return res.status(401).json(createResponse('Error', 'Authorization token is required'));
        }
        try {
            const decoded = jwt.verify(token, process.env.JWT_SECRET);
            const userId = decoded.userId;
            console.log(userId);
            const newArt = await prisma.art.create({
                data: {
                    UserId: parseInt(userId),
                    Image: Image,
                    Description: Description,
                    Price: parseFloat(Price)
                }
            });
            res.status(201).json(createResponse('Success', 'Art piece created successfully'));
        } catch (error) {
            res.status(400).json(createResponse('Error', 'Failed to create art piece' + error.message));
        }
    },

    async buyArt(req, res) {
        const artId = parseInt(req.params.artId);
        const token = req.headers.authorization?.split(' ')[1];

        if (!token) {
            return res.status(401).json(createResponse('Error', 'Authorization token is required'));
        }

        try {
            const decoded = jwt.verify(token, process.env.JWT_SECRET);
            const userId = decoded.userId;
            console.log(userId);
            const art = await prisma.art.findUnique({
                where: { Id: artId }
            });

            if (!art) {
                return res.status(404).json(createResponse('Error', 'Art piece not found'));
            }

            if (art.UserId === userId) {
                return res.status(400).json(createResponse('Error', 'You already own this art piece'));
            }

            const buyer = await prisma.user.findUnique({
                where: { Id: userId }
            });

            if (buyer.Balance < art.Price) {
                return res.status(400).json(createResponse('Error', "You don't have enough money"));
            }

            const owner = await prisma.user.findUnique({
                where: { Id: art.UserId }
            });

            await prisma.$transaction([
                prisma.user.update({
                    where: { Id: userId },
                    data: { Balance: buyer.Balance - art.Price }
                }),
                prisma.user.update({
                    where: { Id: owner.Id },
                    data: { Balance: owner.Balance + art.Price }
                }),
                prisma.art.update({
                    where: { Id: artId },
                    data: { UserId: userId }
                })
            ]);

            res.json(createResponse('Success', 'Art piece purchased successfully'));
        } catch (error) {
            res.status(500).json(createResponse('Error', 'Failed to purchase art piece' + error.message));
        }
    }
};

module.exports = artController;