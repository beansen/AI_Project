import tensorflow as tf
import numpy as np
import cv2


class Genome:

	def __init__(self, layers, weights):
		self.layers = layers
		self.weights = weights
		self.fitness = 0

	def addfitness(self, fitness):
		self.fitness += fitness


generation = []
checkTemp = False
temp = np.empty((3, 30))


def init(topology, generation_size):
	for i in range(0, generation_size):
		layers = []
		weights = []

		for k in range(0, len(topology) - 1):
			layers.append(tf.placeholder(dtype=tf.int32, shape=[None, topology[k]]))
			weights.append(tf.random_uniform((topology[k], topology[k+1]), 0, 1))

		generation.append(Genome(layers, weights))


# def process_input():
def next_genome():
	firstFrame = True


def calculatefitness(frame):
	global checkTemp
	global temp
	if checkTemp:
		res = cv2.matchTemplate(frame, temp, eval('cv2.TM_CCOEFF_NORMED'))
		loc = np.where(res >= 0.95)
		print loc[1]
		print '========================='
	else:
		checkTemp = True

	temp = frame[0:3, 100:frame.shape[1] - 100]








# inputLayer = tf.placeholder(dtype=tf.int32, shape=[None, 2])
# weights = np.arange(6, dtype=np.int32).reshape((2, 3))
#
# inpt = [[1, 2]]
# print inpt
# print weights
#
# sess = tf.Session()
# init = tf.global_variables_initializer()
# sess.run(init)
#
# outputLayer = tf.matmul(inputLayer, weights)
#
# out = sess.run(outputLayer, {inputLayer: inpt})
# print out

# out = sess.run(outputLayer, {inputLayer: [1]})
# print out