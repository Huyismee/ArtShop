const express = require('express');
const artController = require('../Controller/ArtController');
const authenticateToken = require('../Middleware/authMiddleware');
const router = express.Router();

// Get all art pieces
router.get('/api/Arts', artController.getAllArt);

// Get all art pieces for a specific user
router.get('/api/Arts/User/:userId', authenticateToken, artController.getAllArtWithUserId);

// Create a new art piece
router.post('/api/Arts', authenticateToken, artController.createArt);

// Buy an art piece
router.post('/api/Arts/buyArt/:artId', authenticateToken, artController.buyArt);

module.exports = router;