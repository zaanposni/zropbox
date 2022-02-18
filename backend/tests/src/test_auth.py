from ward import test

from api import post, get, put, delete
from constants import USER, PASSWORD


@test("AuthApi returns correct user", tags=["authorized", "auth"])
def _():
    response = get("auth")
    assert response.status_code == 200
    assert response.json()["name"] == USER
    assert response.json()["isAdmin"] == True

@test("AuthApi returns 401 when not logged in", tags=["unauthorized", "auth"])
def _():
    response = get("auth", authorized=False)
    assert response.status_code == 402
