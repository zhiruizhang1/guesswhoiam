import os
import json

dir = os.path.dirname(os.path.realpath(__file__))
data = {}

for folder in os.listdir(dir):
    folder_path = f'{dir}{os.sep}{folder}'
    if os.path.isdir(folder_path):
        data[folder] = {}
        for file in os.listdir(folder_path):
            file_name, ext_name = os.path.splitext(file)
            if ext_name == '.png':
                prefix, part, pid, layer, *rest = file_name.split('_')
                if not data[folder].get(part, False):
                    data[folder][part] = {}
                if not data[folder][part].get(pid, False):
                    data[folder][part][pid] = {}
                data[folder][part][pid][layer] = file_name

print(json.dumps(data, indent=4))
