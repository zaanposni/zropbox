from ward import test

from api import post, get, put, delete
from constants import USER, PASSWORD


@test("LoginApi returns token on valid login", tags=["unauthorized", "login"])
def _():
    response = post("auth/login", {"username": USER, "password": PASSWORD}, False)
    assert response.status_code == 200

@test("LoginApi returns token on valid login - wrong spacing in username", tags=["unauthorized", "login"])
def _():
    response = post("auth/login", {"username": USER.upper(), "password": PASSWORD}, False)
    assert response.status_code == 200

@test("LoginApi returns 401 with wrong password", tags=["unauthorized", "login"])
def _():
    response = post("auth/login", {"username": USER, "password": PASSWORD + "x"}, False)
    assert response.status_code == 401
