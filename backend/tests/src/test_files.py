from datetime import datetime, timedelta

from ward import test
from ward.config import Config
from ward.hooks import hook
from dateutil.parser import parse as dateparse

from api import post, get, put, delete, upload
from constants import USER, PASSWORD

@test("FilesApi should allow upload of all file sizes - 0 bytes", tags=["authorized", "upload", "files"])
def _():
    data = {
        "name": "empty",
        "isPublic": True
    }

    response = upload("files/0", data, "data/empty", "File", authorized=True)

    assert response.status_code == 200

    assert response.json()["name"] == "empty"
    assert response.json()["size"] == 0
    assert response.json()["isPublic"] == True
    assert response.json()["isDir"] == False
    assert response.json()["isRoot"] == True
    assert response.json()["uploadedBy"] == USER

    start = datetime.utcnow() - timedelta(seconds=10)
    end = datetime.utcnow() + timedelta(seconds=10)

    assert dateparse(response.json()["uploadedAt"].split('.')[0]) >= start
    assert dateparse(response.json()["uploadedAt"].split('.')[0]) <= end

@test("FilesApi should allow upload of all file sizes - random image", tags=["authorized", "upload", "files"])
def _():
    data = {
        "name": "random.jpg",
        "isPublic": True
    }

    response = upload("files/0", data, "data/random.jpg", "File", authorized=True)

    assert response.status_code == 200

    assert response.json()["name"] == "random.jpg"
    assert response.json()["size"] == 42159
    assert response.json()["isPublic"] == True
    assert response.json()["isDir"] == False
    assert response.json()["isRoot"] == True
    assert response.json()["uploadedBy"] == USER

    start = datetime.utcnow() - timedelta(seconds=10)
    end = datetime.utcnow() + timedelta(seconds=10)

    assert dateparse(response.json()["uploadedAt"].split('.')[0]) >= start
    assert dateparse(response.json()["uploadedAt"].split('.')[0]) <= end

@test("FilesApi should allow upload of all file sizes - random image", tags=["authorized", "upload", "files"])
def _():
    data = {
        "name": "bigfile",
        "isPublic": True
    }

    response = upload("files/0", data, "data/bigfile", "File", authorized=True)

    assert response.status_code == 200

    assert response.json()["name"] == "bigfile"
    assert response.json()["size"] == 104857600
    assert response.json()["isPublic"] == True
    assert response.json()["isDir"] == False
    assert response.json()["isRoot"] == True
    assert response.json()["uploadedBy"] == USER

    start = datetime.utcnow() - timedelta(minutes=1)
    end = datetime.utcnow() + timedelta(seconds=10)

    assert dateparse(response.json()["uploadedAt"].split('.')[0]) >= start
    assert dateparse(response.json()["uploadedAt"].split('.')[0]) <= end
