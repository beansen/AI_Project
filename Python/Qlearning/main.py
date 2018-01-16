import gym
import numpy as np
import tensorflow as tf

env = gym.make('Breakout-v0')

df = 0.9

s = env.reset()

ipt= tf.constant(s, dtype=tf.float32)
# (210, 160, 3)
conv = tf.contrib.layers.conv2d(ipt, 32, 8, 4, activation_fn=tf.nn.relu)

print conv.shape


