const { PrismaClient } = require('@prisma/client');
const jwt = require('jsonwebtoken');
const {createLoginResponse, createResponse} = require('../Middleware/CreateResponse');
const prisma = new PrismaClient();

const userController = {
  // Register a new user
  async register(req, res) {
    const { email, username, password } = req.body;
    try {
        const existingUser = await prisma.user.findFirst({
            where: {
              OR: [
                { Email: email },
                { Username: username }
              ]
            }
          });
    
          if (existingUser) {
            if (existingUser.Email === email) {
              return res.status(400).json(createResponse('Error', 'Email already registered'));
            }
            if (existingUser.Username === username) {
              return res.status(400).json(createResponse('Error', 'Username already in use'));
            }
          }
    
          // If no existing user found, create the new user
          const newUser = await prisma.user.create({
            data: {
              Email: email,
              Username: username,
              PasswordHash: password, // Note: In a real application, you should hash this password
              Balance: 0 // Setting initial balance to 0
            },
          });
          res.status(201).json(createResponse('Success', 'User Created'));
    } catch (error) {
      res.status(400).json(createResponse('Error', 'Registration failed'));
    }
  },

  // Login user
  async login(req, res) {
    const password= req.body.password;
    const username  = req.body.username;
    console.log(req.body);
    
    try {
      const user = await prisma.user.findFirst({
        where: {
            OR: [
              { Username: username },
              { Email: username }
            ]
          }
      });

      if (!user || user.PasswordHash !== password) { // In a real app, use proper password comparison
        return res.status(401).json(createResponse('Error', 'Invalid password'));
      }

      const token = jwt.sign(
        { userId: user.Id, email: user.Email },
        process.env.JWT_SECRET,
        { expiresIn: '1h' }
      );
      var now = new Date(); // Get current date and time  
      console.log(now.toLocaleString());
      now.setHours(now.getHours() + 1); // Add 1 hour  
      console.log(now.toLocaleString());
      res.json(createLoginResponse(token, now, user.Id));
    } catch (error) {
      res.status(500).json(createResponse('Error', 'Login failed' + error.message));
    }
  },

  // Get user info
  async getUserInfo(req, res) {
    try {
    const { id } = req.params
    var intId = parseInt(id, 10);
    console.log(id);
      const user = await prisma.user.findUnique({
        where: { Id:  intId},
        select: {
          Id: true,
          Email: true,
          Username: true,
          Balance: true,
          Art: true
        }
      });

      if (!user) {
        return res.status(404).json(createResponse('Error', 'Authorization token is required'));
      }

      res.json(user);
    } catch (error) {
      res.status(500).json(createResponse('Error', 'Failed to retrive user info ' + error.message));
    }
  }
};

module.exports = userController;