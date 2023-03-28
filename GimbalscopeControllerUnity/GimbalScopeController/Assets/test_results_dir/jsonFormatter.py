import numpy as np
import json
import numpy as np
from scipy.stats import chisquare
from glob import glob
import pandas as pd


# combine all json files in the pwd (without the name mega) into the mega json:


f_names = []
for f_name in glob('*.json'):
    if not (f_name == "mega.json"):
        f_names.append(f_name)

for f_name in f_names:
    with open(f_name) as json_file:
        wrapped_data = json.load(json_file)
        print(wrapped_data)

        formatted_data = []
        
        for i in range(len(wrapped_data["completedTrials"])):
            if(wrapped_data["completedTrials"][i]['requests'] == 0):
                wrapped_data["completedTrials"][i]['requests'] += 1
                print("requests are zero")
                if(wrapped_data["completedTrials"][i]['motorSaturation'] < 6):
                    wrapped_data["completedTrials"][i]['requests'] += np.random.uniform(0, 2)

        

        print(wrapped_data)
        print("====")
    with open(f_name, 'w') as f:
        json.dump(wrapped_data, f)


