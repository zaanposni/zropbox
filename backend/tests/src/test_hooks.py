from ward.config import Config
from ward.hooks import hook

from api import post, get, put, delete
from constants import USER, PASSWORD, API_TOKEN_FILE

@hook
def before_session(config: Config):
    response = post("auth/login", {"username": USER, "password": PASSWORD})
    assert response.status_code == 200
    with open(API_TOKEN_FILE, "w") as f:
        f.write(response.json()["token"])