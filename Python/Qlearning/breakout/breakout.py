import gym
import numpy as np
import tensorflow as tf
import os

env = gym.make('Breakout-v0')

episodes = 200
stepsPerEpisode = 70
discountFactor = 0.97
chanceRandomAction = 0.1

state = env.reset()[30:210]

inputLayer = tf.placeholder(tf.float32, [180, 160, 3])

conv1 = tf.layers.conv2d(tf.reshape(inputLayer, [1, 180, 160, 3]), 10, [5, 5])
conv2 = tf.layers.conv2d(conv1, 5, [5, 5], strides=(2, 2))
conv3 = tf.layers.conv2d(conv2, 10, [4, 5], strides=(2, 2))

# possible to add dropout layers
dense1 = tf.layers.dense(inputs=tf.reshape(conv3, [1, 42 * 36 * 10]), units=42 * 36 * 10, activation=tf.nn.relu)
dense2 = tf.layers.dense(dense1, units=1024, activation=tf.nn.relu)
logits = tf.layers.dense(dense2, units=4, activation=tf.nn.relu)
predict = tf.argmax(logits, axis=0)


sess = tf.Session()
sess.run(tf.global_variables_initializer())

highestQ, allQ = sess.run([predict, logits], feed_dict={inputLayer: state})


# (1, 176, 156, 10)
# (1, 86, 76, 5)
# (1, 42, 36, 10)
# (1, 15120)
# (1, 1024)
# (1, 4)