import random
import string

try:
    from rich import print
    from rich.panel import Panel
    from rich.prompt import Prompt, Confirm, IntPrompt
    from rich.console import Console
except ImportError:
    print("Please install the rich module.\npip install rich")
    exit(1)


console = Console()
print(Panel("\nWelcome to [red]zropbox[/red] :eyes: \nBe sure that you are in the root directory of this project when executing this script.", title="ZROPBOX"))

ENV_FILE = {
    "ABSOLUTE_PATH_TO_FILE_UPLOAD": "/filehost/"
}

default_sql = {
    "MYSQL_HOST": "db",
    "MYSQL_PORT": "3306",
    "MYSQL_USER": "root",
    "MYSQL_PASSWORD": "root",
    "MYSQL_DATABASE": "zropbox",
    "MYSQL_ROOT_PASSWORD": "root"
}

domain = Prompt.ask(":question_mark: Enter the url you want to host zropbox on.", default="http://127.0.0.1:5620")
Confirm.ask(":exclamation_mark: [bright_black]Be sure to redirect your reverse proxy correctly[/bright_black].\n[bright_black]The docker container will be listening on local port [/bright_black][bright_green]5620[bright_green].", default=True)
ENV_FILE["META_SERVICE_BASE_URL"] = domain

default = Confirm.ask(":question_mark: Do you want to use recommended values for the remaining settings?", default=True)
if default:
    console.log("[bright_green]Using recommended values for the remaining settings.[/bright_green]\nYou can change them later by editing the file [bright_green]zropbox.env[/bright_green]")
    ENV_FILE.update(default_sql)
    console.log("Setting default sql credentials: [red]root:root[/red]")
    ENV_FILE["LOGIN_DURATION"] = "48"
    console.log("Setting default login duration: [red]48[/red] hours.")
else:
    ENV_FILE["MYSQL_HOST"] = Prompt.ask(":question_mark: Enter the hostname of the mysql server.", default=default_sql["MYSQL_HOST"])
    ENV_FILE["MYSQL_PORT"] = Prompt.ask(":question_mark: Enter the port of the mysql server.", default=default_sql["MYSQL_PORT"])
    ENV_FILE["MYSQL_USER"] = Prompt.ask(":question_mark: Enter the username of the mysql server.", default=default_sql["MYSQL_USER"])
    ENV_FILE["MYSQL_PASSWORD"] = Prompt.ask(":question_mark: Enter the password of the mysql server.", default=default_sql["MYSQL_PASSWORD"])
    ENV_FILE["MYSQL_DATABASE"] = Prompt.ask(":question_mark: Enter the database name of the mysql server.", default=default_sql["MYSQL_DATABASE"])
    ENV_FILE["MYSQL_ROOT_PASSWORD"] = Prompt.ask(":question_mark: Enter the root password of the mysql server.", default=default_sql["MYSQL_ROOT_PASSWORD"])

    ENV_FILE["LOGIN_DURATION"] = IntPrompt.ask(":question_mark: Enter the duration of a login session in hours.", default=48)

# generate random jwt secret key 512 chars long
ENV_FILE["JWT_KEY"] = "".join(random.choice(string.ascii_letters + string.digits) for _ in range(512))
console.log("Generating secret key. This is used to sign the jwt (login) tokens.\nIt is recommended to change this key every week.")

init_pw = Prompt.ask(":question_mark: Enter the password for the initial admin user.\n   If you leave this empty a random one will be generated upon startup and can be found in the docker logs")
if init_pw:
    ENV_FILE["PREF_INIT_PASSWORD"] = init_pw
    console.log(f"You can use this password and the user [red]admin[/red] to login to the website.")

env_string = ""
for key, value in ENV_FILE.items():
    env_string += f"{key}={value}\n"

with open("./.env", "w") as fh:
    fh.write(env_string)

print("\n:+1: [bright_green]You are finished[/bright_green].\n[bright_black]You can execute this script again if you want to change anything.[/bright_black]")
print(f"\n:rocket: [bright_green]You can use \"docker-compose up -d\" to start the application[/bright_green].\n[bright_black]After starting you can access the website at:[/bright_black] {ENV_FILE['META_SERVICE_BASE_URL']}/\n")
