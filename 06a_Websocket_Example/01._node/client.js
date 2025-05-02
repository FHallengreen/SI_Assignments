import { WebSocket } from "ws";

const ws = new WebSocket("ws://localhost:8080");

ws.on("open", () => {
    ws.send("Sending a client message from Node.js");

    ws.on("message", (message) => {
        console.log(`Received message from the server: ${message}`);

        // ws.close();
    });
});