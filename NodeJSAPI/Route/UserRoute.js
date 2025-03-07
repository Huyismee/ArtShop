const express = require('express');
const userController = require('../Controller/UserController');
const authenticateToken = require('../Middleware/authMiddleware');
const router = express.Router();

// User login
router.post('/api/Users/login', userController.login);

// User registration
router.post('/api/Users/register', userController.register);

// Get user info
router.get('/api/Users/:id', userController.getUserInfo);

module.exports = router;