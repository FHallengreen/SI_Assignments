import pymysql
from pymongo import MongoClient
from datetime import datetime
from bson import ObjectId
from dotenv import load_dotenv
import os

# Load environment variables from .env file
load_dotenv()

# MySQL connection configuration from .env
mysql_config = {
    'host': os.getenv('MYSQL_HOST'),
    'user': os.getenv('MYSQL_USER'),
    'password': os.getenv('MYSQL_PASSWORD'),
    'database': os.getenv('MYSQL_DATABASE'),
}

# MongoDB connection configuration from .env
mongo_uri = os.getenv('MONGO_URI', 'mongodb://localhost:27017/')
mongo_client = MongoClient(mongo_uri)
mongo_db = mongo_client[os.getenv('MONGO_DATABASE', 'useraccount_db')]

# Function to convert MySQL TIMESTAMP to MongoDB ISODate-compatible format
def convert_timestamp_to_iso(timestamp):
    if timestamp:
        return datetime.fromisoformat(str(timestamp).replace(' ', 'T'))
    return None

try:
    # Connect to MySQL
    mysql_conn = pymysql.connect(**mysql_config)
    mysql_cursor = mysql_conn.cursor(pymysql.cursors.DictCursor)

    # Drop existing MongoDB collections to ensure clean migration
    mongo_db['roles'].drop()
    mongo_db['users'].drop()
    mongo_db['accounts'].drop()

    # Migrate roles
    print("Migrating roles...")
    mysql_cursor.execute("SELECT * FROM role")
    roles = mysql_cursor.fetchall()
    role_id_map = {}  # Map MySQL role.id to MongoDB ObjectId
    for role in roles:
        mongo_role = {
            '_id': ObjectId(),
            'name': role['name']
        }
        mongo_db['roles'].insert_one(mongo_role)
        role_id_map[role['id']] = mongo_role['_id']
    print(f"Migrated {len(roles)} roles.")

    # Migrate users
    print("Migrating users...")
    mysql_cursor.execute("SELECT * FROM user")
    users = mysql_cursor.fetchall()
    user_id_map = {}  # Map MySQL user.id to MongoDB ObjectId
    for user in users:
        mongo_user = {
            '_id': ObjectId(),
            'username': user['username'],
            'email': user['email'],
            'password': user['password'],
            'created_at': convert_timestamp_to_iso(user['created_at']),
            'updated_at': convert_timestamp_to_iso(user['updated_at']),
            'role_id': role_id_map[user['role_id']]  # Reference MongoDB role _id
        }
        mongo_db['users'].insert_one(mongo_user)
        user_id_map[user['id']] = mongo_user['_id']
    print(f"Migrated {len(users)} users.")

    # Migrate accounts
    print("Migrating accounts...")
    mysql_cursor.execute("SELECT * FROM account")
    accounts = mysql_cursor.fetchall()
    for account in accounts:
        mongo_account = {
            '_id': ObjectId(),
            'name': account['name'],
            'amount': float(account['amount']),  # Convert DECIMAL to float
            'user_id': user_id_map[account['user_id']]  # Reference MongoDB user _id
        }
        mongo_db['accounts'].insert_one(mongo_account)
    print(f"Migrated {len(accounts)} accounts.")

    # Verify migration by printing sample data from MongoDB
    print("\nSample data in MongoDB:")
    print("Roles:", list(mongo_db['roles'].find({}, {'_id': 0, 'name': 1})))
    print("Users:", list(mongo_db['users'].find({}, {'_id': 0, 'username': 1, 'email': 1, 'role_id': 1})))
    print("Accounts:", list(mongo_db['accounts'].find({}, {'_id': 0, 'name': 1, 'amount': 1, 'user_id': 1})))

except Exception as e:
    print(f"Error during migration: {e}")

finally:
    # Close connections
    if 'mysql_cursor' in locals():
        mysql_cursor.close()
    if 'mysql_conn' in locals():
        mysql_conn.close()
    if 'mongo_client' in locals():
        mongo_client.close()