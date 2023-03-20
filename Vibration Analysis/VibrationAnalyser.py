import numpy as np
from scipy.fft import fft, rfft
from scipy.fft import fftfreq, rfftfreq
import plotly.graph_objs as go
from plotly.subplots import make_subplots
import matplotlib.pyplot as plt

filename = "data_text.txt"
file_contents = []


with open(filename, 'r') as f:
    file_contents = f.readlines()

sorted_file_contents = []
for record in file_contents:
    if(not (record == "\n")):
        sorted_file_contents.append(record.strip())

sorted_file_contents.pop(0)

def Extract_Var_and_Time(vibrations_list, var_label):
    timestamp_var_pairs = []
    for record in vibrations_list:
        items = record.split(",")
        pair = []
        for item in items:
            
            if(item[0] == var_label or item[0] == 't'):
                var_and_value = item.split(":")
                var = var_and_value[1]
                pair.append(var)
        if(len(pair) == 2):
            timestamp_var_pairs.append(pair)
    return timestamp_var_pairs
            
            

sample_rate = 900
# now extract only the Xs
x_and_time = Extract_Var_and_Time(sorted_file_contents, 'x')
y_and_time = Extract_Var_and_Time(sorted_file_contents, 'y')
z_and_time = Extract_Var_and_Time(sorted_file_contents, 'z')

x_values = [float(x[0]) for x in x_and_time]
y_values = [float(x[0]) for x in y_and_time]
z_values = [float(x[0]) for x in z_and_time]
# plot in time domain
# test plot
plt.plot(x_values)
plt.plot(y_values)
plt.plot(z_values)
plt.show()


# Apply the FFT on the signal
# Calculate N/2 to normalize the FFT output
N = len(x_values)

# Plot the result (the spectrum |Xk|)
plt.plot(rfftfreq(N, d=1/sample_rate), 2*np.abs(rfft(x_values))/N, linewidth=0.2)
plt.plot(rfftfreq(N, d=1/sample_rate), 2*np.abs(rfft(y_values))/N, linewidth=0.2)
plt.plot(rfftfreq(N, d=1/sample_rate), 2*np.abs(rfft(z_values))/N, linewidth=0.2)


z = np.polyfit(rfftfreq(N, d=1/sample_rate), 2*np.abs(rfft(x_values))/N, 3)

plt.ylabel('Amplitude')
plt.xlabel('Frequency[Hz]')
plt.title('Spectrum')
plt.show()















# now plot 

time_values = [x[1] for x in x_and_time]
x_values = [float(x[0]) for x in x_and_time]

x_dx = np.gradient(x_values,20)
x_dx_abs = [abs(x) for x in x_dx]
plt.plot(x_dx)
plt.show()