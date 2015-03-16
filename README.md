# IoTDemo

En Mobile Services crear una table EventLog con una columna Description.

En el script de inserción configurar lo siguiente:

//Ejemplo desde azure.com

function insert(item, user, request) {
// Define a payload for the Google Cloud Messaging toast notification.
var payload = {
    data: {
        message: item.Description 
    }
};      
request.execute({
    success: function() {
        // If the insert succeeds, send a notification.
        push.gcm.send(null, payload, {
            success: function(pushResponse) {
                console.log("Sent push:", pushResponse, payload);
                request.respond();
                },              
            error: function (pushResponse) {
                console.log("Error Sending push:", pushResponse);
                request.respond(500, { error: pushResponse });
                }
            });
        },
    error: function(err) {
        console.log("request.execute error", err)
        request.respond();
    }
  });
}
