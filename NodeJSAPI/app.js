const express = require("express");
const dotenv = require("dotenv");
const artRoutes = require('./Route/ArtRoute');
const userRoutes = require('./Route/UserRoute');
dotenv.config();

const app = express();
app.use(express.json())
app.use(express.urlencoded({ extended: true }))
// config connect db
// connnect();

// import routes

// routes(app)


app.use(artRoutes);
app.use(userRoutes);
const PORT = process.env.PORT
app.listen(PORT, () => {
    console.log("Server is running on port: http://localhost:" + PORT); 
})