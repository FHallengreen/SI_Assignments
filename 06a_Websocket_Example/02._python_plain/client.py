# from websockets.sync.client import connect

# def hello():
#     with connect("ws://localhost:8000") as websocket:
#         websocket.send("Hello world!")
#         message = websocket.recv()
#         print(f"Received: {message}")

# hello()

import asyncio
import websockets

async def send_message():
    uri = "ws://localhost:8000"
    async with websockets.connect(uri) as websocket:
        await websocket.send("This is my message")
        print(await websocket.recv())

asyncio.run(send_message())