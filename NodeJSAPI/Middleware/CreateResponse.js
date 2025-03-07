
function createLoginResponse(Token, Expiration, UserId) {
    return { Token, Expiration, UserId };
  }
function createResponse(Status, Message) {
    return { Status, Message };
  }

  
  module.exports = {createLoginResponse, createResponse}