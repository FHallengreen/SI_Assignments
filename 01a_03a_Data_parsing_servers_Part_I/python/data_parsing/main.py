import os
import json
import csv
import xml.etree.ElementTree as ET
import yaml

class FileReader:
    def __init__(self, base_path="../../datasets"):
        self.base_path = base_path

    def read_file(self, filename):
        full_path = os.path.join(self.base_path, filename)
        with open(full_path, "r") as file:
            return file.read()

    def read_txt(self):
        content = self.read_file("product.txt")
        products = []
        current_product = {}
        
        for line in content.split("\n"):
            if line.strip():
                key, value = line.split("=", 1)
                if key == "id":
                    if current_product:
                        products.append(current_product)
                    current_product = {"id": int(value)}
                elif key == "name":
                    current_product["name"] = value
                elif key == "price":
                    current_product["price"] = float(value)
        
        if current_product:
            products.append(current_product)
        
        return products

    def read_csv(self):
        full_path = os.path.join(self.base_path, "product.csv")
        with open(full_path, "r", newline="") as csvfile:
            reader = csv.DictReader(csvfile)
            return [
                {
                    "id": int(row["id"]),
                    "name": row["name"],
                    "price": float(row["price"]),
                }
                for row in reader
            ]

    def read_json(self):
        full_path = os.path.join(self.base_path, "product.json")
        with open(full_path, "r") as file:
            data = json.load(file)
            return [
                {
                    "id": int(item["id"]),
                    "name": item["name"],
                    "price": float(item["price"]),
                }
                for item in data
            ]

    def read_xml(self):
        full_path = os.path.join(self.base_path, "product.xml")
        tree = ET.parse(full_path)
        root = tree.getroot()
        products = []
        for product in root.findall("Product"):
            products.append({
                "id": int(product.find("id").text),
                "name": product.find("name").text,
                "price": float(product.find("price").text),
            })
        return products

    def read_yaml(self):
        full_path = os.path.join(self.base_path, "product.yaml")
        with open(full_path, "r") as file:
            data = yaml.safe_load(file)
            products = data.get("products", [])
            return [
                {
                    "id": int(item["id"]),
                    "name": item["name"],
                    "price": float(item["price"]),
                }
                for item in products
            ]

if __name__ == "__main__":
    reader = FileReader()
    print("TXT Data:", reader.read_txt())
    print("CSV Data:", reader.read_csv())
    print("JSON Data:", reader.read_json())
    print("XML Data:", reader.read_xml())
    print("YAML Data:", reader.read_yaml())