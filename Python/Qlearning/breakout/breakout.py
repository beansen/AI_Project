import gym
import numpy as np
import tensorflow as tf
import os

env = gym.make('Breakout-v0')

episodes = 10
stepsPerEpisode = 70
discountFactor = 0.9
chanceRandomAction = 0.1


inputLayer = tf.placeholder(tf.float32, [180, 160, 3])

conv1 = tf.layers.conv2d(tf.reshape(inputLayer, [1, 180, 160, 3]), 10, [5, 5])
conv2 = tf.layers.conv2d(conv1, 5, [5, 5], strides=(2, 2))
conv3 = tf.layers.conv2d(conv2, 10, [4, 5], strides=(2, 2))

# possible to add dropout layers
dense1 = tf.layers.dense(tf.reshape(conv3, [1, 42 * 36 * 10]), units=42 * 36 * 10, activation=tf.nn.relu)
dense2 = tf.layers.dense(dense1, units=1024, activation=tf.nn.relu)
logits = tf.layers.dense(dense2, units=4, activation=tf.nn.relu)
predict = tf.argmax(logits[0])

newQ = tf.placeholder(shape=[1, 4], dtype=tf.float32)
loss = tf.reduce_sum(tf.square(newQ - logits))
trainer = tf.train.GradientDescentOptimizer(learning_rate=0.1)
updateModel = trainer.minimize(loss)


sess = tf.Session()
sess.run(tf.global_variables_initializer())

for i in range(episodes):
	currentState = env.reset()[30:210] / 255.0

	for k in range(stepsPerEpisode):
		action, qValues = sess.run([predict, logits], feed_dict={inputLayer: currentState})
		if np.random.rand(1) <= chanceRandomAction:
			action = env.action_space.sample()

		nextState, reward, done, info = env.step(action)

		nextAction, nextQ = sess.run([predict, logits], feed_dict={inputLayer: nextState[30:210] / 255.0})
		qValues[0, action] = reward + discountFactor * nextQ[0, nextAction]

		sess.run([updateModel], feed_dict={inputLayer: currentState, newQ: qValues})

		currentState = nextState[30:210]
