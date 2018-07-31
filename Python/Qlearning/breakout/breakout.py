import gym
import numpy as np
import tensorflow as tf
import os

env = gym.make('Breakout-v1')

episodes = 10
stepsPerEpisode = 70
discountFactor = 0.9
keepRate = 0.5

currentReward = 0

inputLayer = tf.placeholder(tf.float32, [180, 160, 3])

conv1 = tf.layers.conv2d(tf.reshape(inputLayer, [1, 180, 160, 3]), 10, [5, 5])
conv2 = tf.layers.conv2d(conv1, 5, [5, 5], strides=(2, 2))
conv3 = tf.layers.conv2d(conv2, 10, [4, 5], strides=(2, 2))

pKeep = tf.placeholder(tf.float32)

dense1 = tf.layers.dense(tf.reshape(conv3, [1, 42 * 36 * 10]), units=42 * 36 * 10, activation=tf.nn.relu)
dropDense1 = tf.layers.dropout(dense1, pKeep)
dense2 = tf.layers.dense(dropDense1, units=1024, activation=tf.nn.relu)
dropDense2 = tf.layers.dropout(dense2, pKeep)
logits = tf.layers.dense(dropDense2, units=4, activation=tf.nn.relu)
predict = tf.argmax(logits[0])

newQ = tf.placeholder(shape=[1, 4], dtype=tf.float32)
loss = tf.reduce_sum(tf.squared_difference(newQ, logits))
trainer = tf.train.GradientDescentOptimizer(learning_rate=0.3)
updateModel = trainer.minimize(loss)

tf.summary.scalar('loss', loss)
tf.summary.scalar('reward', currentReward)

summary_op = tf.summary.merge_all()

sess = tf.Session()
sess.run(tf.global_variables_initializer())

filewriter = tf.summary.FileWriter(os.getcwd() + '/trainingdata', sess.graph)

for i in range(episodes):
	currentState = env.reset()[30:210] / 255.0

	env.render()

	for k in range(stepsPerEpisode):
		action, qValues = sess.run([predict, logits], feed_dict={inputLayer: currentState, pKeep: keepRate})

		nextState, reward, done, info = env.step(action)
		env.render()

		print 'ep:{} step:{} - {}'.format(i, k, reward)
		currentReward = reward

		nextAction, nextQ = sess.run([predict, logits], feed_dict={inputLayer: nextState[30:210] / 255.0, pKeep: keepRate})
		qValues[0, action] = reward + discountFactor * nextQ[0, nextAction]

		_, summary = sess.run([updateModel, summary_op], feed_dict={inputLayer: currentState, newQ: qValues})

		filewriter.add_summary(summary, i * stepsPerEpisode + k)

		currentState = nextState[30:210] / 255.0
