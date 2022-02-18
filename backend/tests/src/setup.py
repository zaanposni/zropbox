from api import post, get, put, delete
from constants import USER, PASSWORD, API_TOKEN_FILE

response = post("auth/login", {"username": USER, "password": PASSWORD}, False)
assert response.status_code == 200
with open(API_TOKEN_FILE, "w") as f:
    f.write(response.json()["token"])
