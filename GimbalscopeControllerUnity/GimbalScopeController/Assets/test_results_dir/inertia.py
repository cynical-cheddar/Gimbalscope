import numpy as np
import sympy as smp
import matplotlib.pyplot as plt
from scipy.integrate import dblquad
from scipy.integrate import tplquad
import pandas as pd
from IPython.display import display
from mpl_toolkits.mplot3d.axes3d import Axes3D
from mpl_toolkits.mplot3d import proj3d
from tqdm import tqdm





pathTop = r'D:\Repos\Gimbalscope\Gimbalscope\GimbalscopeControllerUnity\GimbalScopeController\Assets\test_results_dir\top_126x57x54.txt'
pathMid = r'D:\Repos\Gimbalscope\Gimbalscope\GimbalscopeControllerUnity\GimbalScopeController\Assets\test_results_dir\mid_52x53x168.txt'
pathBot = r'D:\Repos\Gimbalscope\Gimbalscope\GimbalscopeControllerUnity\GimbalScopeController\Assets\test_results_dir\bot_91x168x87.txt'
pathWhole = r'D:\Repos\Gimbalscope\Gimbalscope\GimbalscopeControllerUnity\GimbalScopeController\Assets\test_results_dir\bot mid top_107x97x168.txt'

topMass = 0.350
midMass = 0.150
botMass = 0.412
#coords[[1, 2]] = coords[[2, 1]]
#coords = coords/max(coords.ravel())

# TOP COMPONENT VISUALISATION==========================================================
zs_translate_si = 0.1 - 0.025
voxel_dimensions_z = 54
height_SI = 0.07
voxelsTop = np.loadtxt(pathTop, unpack=True, delimiter=',', dtype=int)
xs, ys, zs, a, b, c  = voxelsTop
sf = (height_SI / voxel_dimensions_z)
# voxels are now in si units
top_xs_si, top_ys_si, top_zs_si, a, b, c  = voxelsTop*sf 
top_xs_si_calc, top_ys_si_calc, top_zs_si_calc, a, b, c  = voxelsTop*sf 
top_zs_si = top_zs_si + zs_translate_si
top_zs_si*= 2
top_xs_si -= 0.09
top_ys_si -= 0.05
# MID COMPONENT VISUALISATION==========================================================
zs_translate_si = 0- 0.025
xs_translate_si = 0.072
ys_translate_si = -0.01
voxel_dimensions_z = 168
height_SI = 0.13
voxelsMid = np.loadtxt(pathMid, unpack=True, delimiter=',', dtype=int)
xs, ys, zs, a, b, c  = voxelsMid
sf = (height_SI / voxel_dimensions_z)
# voxels are now in si units
mid_xs_si, mid_ys_si, mid_zs_si, a, b, c  = voxelsMid*sf 
mid_xs_si_calc, mid_ys_si_calc, mid_zs_si_calc, a, b, c  = voxelsMid*sf 
mid_xs_si = mid_xs_si + xs_translate_si
mid_ys_si = mid_ys_si + ys_translate_si
mid_zs_si = mid_zs_si + zs_translate_si
mid_zs_si *= 2
mid_xs_si -= 0.09
mid_ys_si -= 0.05
# BOT COMPONENT VISUALISATION==========================================================
zs_translate_si = -0.075- 0.025
xs_translate_si = 0.04
ys_translate_si = -0.035
voxel_dimensions_z = 87
height_SI = 0.075
voxelsBot = np.loadtxt(pathBot, unpack=True, delimiter=',', dtype=int)
xs, ys, zs, a, b, c  = voxelsBot
sf = (height_SI / voxel_dimensions_z)
# voxels are now in si units
bot_xs_si, bot_ys_si, bot_zs_si, a, b, c  = voxelsBot*sf 
bot_xs_si_calc, bot_ys_si_calc, bot_zs_si_calc, a, b, c  = voxelsBot*sf
bot_xs_si = bot_xs_si + xs_translate_si
bot_ys_si = bot_ys_si + ys_translate_si
bot_zs_si = bot_zs_si + zs_translate_si
bot_zs_si *= 2
bot_xs_si -= 0.09
bot_ys_si -= 0.06
# EVERYTHING COMPONENT =======================================================
voxel_dimensions_z = 168
height_SI = 0.3
voxelsWhole = np.loadtxt(pathWhole, unpack=True, delimiter=',', dtype=int)
xs, ys, zs, a, b, c  = voxelsWhole
sf = (height_SI / voxel_dimensions_z)
# voxels are now in si units
xs_si_calc, ys_si_calc, zs_si_calc, a, b, c  = voxelsWhole*sf
# transform
x_t = 0
y_t = 0
z_t = -0.15
xs_si_calc += x_t
ys_si_calc += y_t
zs_si_calc += z_t
# view position
fig = plt.figure()
ax = fig.add_subplot(projection='3d')


xs_si_calc -= 0.075
ys_si_calc -= 0.1
ax.scatter(xs = xs_si_calc,ys= ys_si_calc,zs = zs_si_calc, c = 'blue', s = 0.01)



ax.view_init(elev=20)
plt.show()



zs_si_calc, ys_si_calc, xs_si_calc = zip(*sorted(zip(zs_si_calc, ys_si_calc, xs_si_calc)))

n = len(zs_si_calc)
print(n)

lowerBound = 0.075 + z_t
upperBound = 0.21 + z_t
Ixx_sum_bot = 0
Iyy_sum_bot = 0
Izz_sum_bot = 0
n_bot = 0

Ixx_sum_mid = 0
Iyy_sum_mid = 0
Izz_sum_mid = 0
n_mid = 0

Ixx_sum_top = 0
Iyy_sum_top = 0
Izz_sum_top = 0
n_top = 0
s = 0

for (x, y, z) in zip(xs_si_calc, ys_si_calc, zs_si_calc):
    if(z <lowerBound):
        Ixx_sum_bot += y**2 + z**2
        Iyy_sum_bot += x**2 + z**2
        Izz_sum_bot += z**2 + y**2
        n_bot += 1
    elif(z > lowerBound and z < upperBound):
        Ixx_sum_mid += y**2 + z**2
        Iyy_sum_mid += x**2 + z**2
        Izz_sum_mid += z**2 + y**2
        n_mid += 1
    else:
        Ixx_sum_top += y**2 + z**2
        Iyy_sum_top += x**2 + z**2
        Izz_sum_top += z**2 + y**2
        n_top += 1

print(n_bot)
print(n_mid)
print(n_top)
Ix = ((Ixx_sum_bot/n_bot) * botMass) + ((Ixx_sum_mid/n_mid) * midMass) + ((Ixx_sum_top/n_top)*topMass)
Iy = ((Iyy_sum_bot/n_bot) * botMass) + ((Iyy_sum_mid/n_mid) * midMass) + ((Iyy_sum_top/n_top)*topMass)
Iz = ((Izz_sum_bot/n_bot) * botMass) + ((Izz_sum_mid/n_mid) * midMass) + ((Izz_sum_top/n_top)*topMass)
print("Ixx")
print(Ix)
print("Iyy")
print(Iy)
print("Izz")
print(Iz)



fig = plt.figure()
ax = fig.add_subplot(projection='3d')

ax.scatter(xs = bot_xs_si,ys= bot_ys_si,zs = bot_zs_si, c = 'red', s = 0.01)
ax.scatter(xs = mid_xs_si,ys= mid_ys_si,zs = mid_zs_si, c = 'green', s = 0.01)
ax.scatter(xs = top_xs_si,ys= top_ys_si,zs = top_zs_si, c = 'blue', s = 0.2)
# scale
x_scale=1
y_scale=1
z_scale=1.6

scale=np.diag([x_scale, y_scale, z_scale, 1.0])
scale=scale*(1.0/scale.max())
scale[3,3]=1.0

def short_proj():
  return np.dot(Axes3D.get_proj(ax), scale)

ax.get_proj=short_proj


# end scale

ax.view_init(elev=20)
plt.show()

input()

Ixx = 0
Iyy = 0
Izz = 0

# TOP MASS
N = voxelsTop.shape[1]
Ixx += (sum(top_ys_si_calc**2 + top_zs_si_calc**2)/N)*topMass
Iyy += (sum(top_xs_si_calc**2 + top_zs_si_calc**2)/N)*topMass
Izz += (sum(top_xs_si_calc**2 + top_ys_si_calc**2)/N)*topMass

# MID MASS
N = voxelsMid.shape[1]
Ixx += (sum(mid_ys_si_calc**2 + mid_zs_si_calc**2)/N)*midMass
Iyy += (sum(mid_xs_si_calc**2 + mid_zs_si_calc**2)/N)*midMass
Izz += (sum(mid_xs_si_calc**2 + mid_ys_si_calc**2)/N)*midMass

# BOT MASS
N = voxelsBot.shape[1]
Ixx += (sum(bot_ys_si_calc**2 + bot_zs_si_calc**2)/N)*botMass
Iyy += (sum(bot_xs_si_calc**2 + bot_zs_si_calc**2)/N)*botMass
Izz += (sum(bot_xs_si_calc**2 + bot_ys_si_calc**2)/N)*botMass



