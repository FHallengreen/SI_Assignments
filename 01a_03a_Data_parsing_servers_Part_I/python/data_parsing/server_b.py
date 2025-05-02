import requests
from fastapi import FastAPI
from pydantic import BaseModel
from typing import List
from main import FileReader

app = FastAPI()
reader = FileReader()

SERVER_A_URL = "http://localhost:5000/api/data"

class Product(BaseModel):
    id: int
    name: str
    price: float

@app.get("/txt", response_model=List[Product])
def get_txt():
    response = requests.get(f"{SERVER_A_URL}/local/text")
    response.raise_for_status()
    return response.json()

@app.get("/csv", response_model=List[Product])
def get_csv():
    response = requests.get(f"{SERVER_A_URL}/local/csv")
    response.raise_for_status()
    return response.json()

@app.get("/json", response_model=List[Product])
def get_json():
    response = requests.get(f"{SERVER_A_URL}/local/json")
    response.raise_for_status()
    return response.json()

@app.get("/xml", response_model=List[Product])
def get_xml():
    response = requests.get(f"{SERVER_A_URL}/local/xml")
    response.raise_for_status()
    return response.json()

@app.get("/yaml", response_model=List[Product])
def get_yaml():
    response = requests.get(f"{SERVER_A_URL}/local/yaml")
    response.raise_for_status()
    return response.json()

@app.get("/local/txt", response_model=List[Product])
def get_local_txt():
    return reader.read_txt()

@app.get("/local/csv", response_model=List[Product])
def get_local_csv():
    return reader.read_csv()

@app.get("/local/json", response_model=List[Product])
def get_local_json():
    return reader.read_json()

@app.get("/local/xml", response_model=List[Product])
def get_local_xml():
    return reader.read_xml()

@app.get("/local/yaml", response_model=List[Product])
def get_local_yaml():
    return reader.read_yaml()

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)