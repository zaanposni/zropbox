from ward import test

from api import post, get, put, delete, upload


@test("FilesApi should return same image on download - authorized access", tags=["authorized", "download", "files"])
def _():
    data = {
        "name": "random.jpg",
        "isPublic": True
    }

    response = upload("files/0", data, "data/random.jpg", "File", authorized=True)

    assert response.status_code == 200

    assert response.json()["name"] == "random.jpg"
    assert response.json()["size"] == 42159

    file_id = response.json()["id"]
    assert str(file_id).isnumeric()

    with open("data/random.jpg", "rb") as fh:
        file_data = fh.read()

    download = get(f"files/{file_id}", authorized=True)

    assert download.content == file_data


@test("FilesApi should return same image on download - anonymous access", tags=["authorized", "unauthorized", "download", "files"])
def _():
    data = {
        "name": "random.jpg",
        "isPublic": True
    }

    response = upload("files/0", data, "data/random.jpg", "File", authorized=True)

    assert response.status_code == 200

    assert response.json()["name"] == "random.jpg"
    assert response.json()["size"] == 42159

    file_id = response.json()["id"]
    assert str(file_id).isnumeric()

    with open("data/random.jpg", "rb") as fh:
        file_data = fh.read()

    download = get(f"files/{file_id}", authorized=False)

    assert download.content == file_data


@test("FilesApi should return 401 on private image", tags=["authorized", "unauthorized", "download", "files"])
def _():
    data = {
        "name": "random.jpg",
        "isPublic": False
    }

    response = upload("files/0", data, "data/random.jpg", "File", authorized=True)

    assert response.status_code == 200

    assert response.json()["name"] == "random.jpg"
    assert response.json()["size"] == 42159

    file_id = response.json()["id"]
    assert str(file_id).isnumeric()

    with open("data/random.jpg", "rb") as fh:
        file_data = fh.read()

    download = get(f"files/{file_id}", authorized=False)

    assert download.status_code == 401
    assert download.content != file_data
