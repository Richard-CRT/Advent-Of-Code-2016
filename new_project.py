import requests
import os
import shutil
import re
import uuid
import json
import browser_cookie3
import html

year = 2016
sln_file = f"Advent_of_Code_{year}.sln"
template_project = "TemplateProject"

def download_input(url, dst_input):    
    input_url = f"{url}/input"
    print(f"Downloading contents for `{dst_input}` from `{input_url}`")
    try:
        cj = browser_cookie3.firefox(domain_name='adventofcode.com')
        resp = requests.get(input_url, cookies=cj)
    except Exception as err:
        print(f"Error downloading input from `{input_url}`, contents not written to `{dst_input}`")
        raise
    else:
        if resp.status_code == 200:
            input_contents = str(resp.text)
            
            print(f"Writing input from `{input_url}` to `{dst_input}`")
            with open(dst_input, 'w') as file:
                file.write(input_contents)
        else:
            print(f"HTTP error code `{resp.status_code}` when attempting to read input (indicates invalid session cookie)")

def new_project(url, new_project_name, dst_input):    
    try:
        with open(sln_file, 'r') as file:
            sln_contents = file.read()
    except FileNotFoundError:
        print (f"Solution file `{sln_file}` not found")
        raise
    else:
        regex = r"(\s*Project\s*?\(\"{[^}]+}\"\)\s*?=\s*?\")" + template_project + r"(\"\s*?,\s*?\")[^\"]+(\"\s*?,\s*?\"{)[^}]+(}\"\s*?EndProject)"
        m = re.search(regex, sln_contents)
        if m:
            print(f"Generating new project definition `{new_project_name}` for `{sln_file}`")
            guid = str(uuid.uuid4()).upper()
            new_proj_def = f"{m.group(1)}{new_project_name}{m.group(2)}{os.path.join(new_project_name, new_project_name + '.csproj')}{m.group(3)}{guid}{m.group(4)}"
            
            print(f"Making directory `{new_project_name}`")
            os.mkdir(new_project_name)
            
            src_input = os.path.join(template_project, "input.txt")
            print(f"Copying `{src_input}` to `{dst_input}`")
            shutil.copyfile(src_input, dst_input)
            src_proj = os.path.join(template_project, "TemplateProject.csproj")
            dst_proj = os.path.join(new_project_name, f"{new_project_name}.csproj")
            print(f"Copying `{src_proj}` to `{dst_proj}`")
            shutil.copyfile(src_proj, dst_proj)
            src_cs = os.path.join(template_project, "Program.cs")
            dst_cs = os.path.join(new_project_name, "Program.cs")
            print(f"Copying `{src_cs}` to `{dst_cs}`")
            shutil.copyfile(src_cs, dst_cs)
            
            print(f"Adding new project `{new_project_name}` to `{sln_file}`")
            replace_regex = r"(.*Project\s*?\(\"{[^}]+}\"\)\s*?=\s*?\"[^\"]+\"\s*?,\s*?\"[^\"]+\"\s*?,\s*?\"{[^}]+}\"\s*?EndProject)"
            new_sln_contents = re.sub(replace_regex, "\\1" + new_proj_def.replace("\\","\\\\"), sln_contents, 1, re.DOTALL)
            
            with open(sln_file, 'w') as file:
                file.write(new_sln_contents)
            
            download_input(url, dst_input)
        else:
            print(f"Project with title `{template_project}` not found in `{sln_file}`")

def main():
    try:
        while True:
            day = int(input("Day: "))
            url = f"https://adventofcode.com/{year}/day/{day}"
            print(f"Fetching HTML from `{url}`")
            try:
                resp = requests.get(url)
            except Exception as err:
                continue
            else:
                break
        html_source = str(resp.text)
        open_del = "<h2>--- "
        close_del = " ---</h2>"
        open_h2 = html_source.index(open_del)
        close_h2 = html_source.index(close_del)
        title = html.unescape(html_source[open_h2 + len(open_del):close_h2])
        print(f"Original title is `{title}`")
        renamed_title = title.replace(" ", "_")
        keep_chars = set("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-&")
        for remove_char in set(renamed_title).difference(keep_chars):
            renamed_title = renamed_title.replace(remove_char, "")
        renamed_title = re.sub(r"Day_(\d_.+)", "Day_0\\1", renamed_title, 1)
        print(f"Suggested project title is `{renamed_title}`")
        r = ""
        while r != "y" and r != "n":
            r = input("Accept suggested title? [Y/n] ").lower()
            if r == "":
                r = "y"
        if r == "n":
            day_string = f"{day:02}"
            r = ""
            while r == "":
                r = input(f"What should the new project be called? Day_{day_string}_")
            r = f"Day_{day_string}_{r}"
        else:
            r = renamed_title
        print(f"Using project name `{r}`")
        print(f"Checking if directory `{r}` already exists")
        dst_input = os.path.join(r, "input.txt")
        if (os.path.exists(r)):
            print(f"Directory `{r}` already exists")
            r = ""
            while r != "y" and r != "n":
                r = input("Do you want to re-download the input.txt anyway? [y/N] ").lower()
                if r == "":
                    r = "n"
            if r == "y":
                download_input(url, dst_input)
        else:
            new_project(url, r, dst_input)
    except Exception as err:
        print(repr(err))
        input("Pausing for operator to review exception... ")

main()
input("Press enter to continue... ")
