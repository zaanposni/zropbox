from rich.console import Console
from rich.progress import track

from api import post, get, put, delete
from constants import USER, PASSWORD, API_TOKEN

console = Console()

console.print("Logging in...", style="bright_black")

response = post("auth/login", {"username": USER, "password": PASSWORD}, False)
assert response.status_code == 200
API_TOKEN = response.json()["token"]

console.print("Logged in!", style="bright_green")
console.print("Cleaning up...", style="bright_black")

all_files = get("directory/0", authorized=True).json()['items']

console.print(f"Deleting all files ({len(all_files)})...", style="bright_black")

for i in track(range(len(all_files)), description="Processing..."):
    delete(f"files/{all_files[i]['id']}", authorized=True)

console.print(f"Deleted {len(all_files)} files.", style="bright_green")
console.print("Done!", style="bright_green")
