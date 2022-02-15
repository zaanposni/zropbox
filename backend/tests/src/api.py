import requests

from constants import API_HOST, API_PATH, API_BEARER, API_TOKEN

def request(method: str, url: str, data: dict = None, authorized: bool = True) -> requests.Response:
    """
    Make a request to the API.
    """
    headers = {
        "Content-Type": "application/json"
    }
    if authorized:
        headers["Authorization"] = API_BEARER + API_TOKEN

    return requests.request(method, f"{API_HOST}{API_PATH}{url}", json=data, headers=headers)

def post(url: str, data: dict = None, authorized: bool = True) -> requests.Response:
    """
    Make a POST request to the API.
    """
    return request("POST", url, data, authorized)

def get(url: str, authorized: bool = True) -> requests.Response:
    """
    Make a GET request to the API.
    """
    return request("GET", url, None, authorized)

def put(url: str, data: dict = None, authorized: bool = True) -> requests.Response:
    """
    Make a PUT request to the API.
    """
    return request("PUT", url, data, authorized)

def delete(url: str, authorized: bool = True) -> requests.Response:
    """
    Make a DELETE request to the API.
    """
    return request("DELETE", url, None, authorized)

def upload(url: str, data: dict, filepath: str, filename: str, authorized: bool = True) -> requests.Response:
    """
    Make a POST request to the API with a file.
    """
    headers = { }
    if authorized:
        headers["Authorization"] = API_BEARER + API_TOKEN

    return requests.post(f"{API_HOST}{API_PATH}{url}", data=data, headers=headers, files={filename: open(filepath, "rb")})
