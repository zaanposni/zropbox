USER           = "zropboxtest"
PASSWORD       = "zropboxtest"

API_HOST       = "http://localhost:5620"
API_PATH       = "/api/"
API_BEARER     = "Bearer "
API_TOKEN      = ""
API_TOKEN_FILE = "data/logindata"

try:
    with open(API_TOKEN_FILE, "r") as f:
        API_TOKEN = f.read()
except FileNotFoundError:
    pass
