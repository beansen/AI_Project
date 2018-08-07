import numpy as np
import cv2
from PIL import ImageGrab
import neural_network as nn
import image_processor as img_processor

img_processor.init()

temp = np.empty((3, 491))
checkTemp = False
imgCounter = 1
while True:
	img_np = np.array(ImageGrab.grab(bbox=(0, 50, 612, 525)))
	cv2.imwrite('mario' + imgCounter.__str__() + '.png', img_np)
	imgCounter += 1
# grayScale = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
# cutout = grayScale[100:grayScale.shape[0]-1, 0:grayScale.shape[1]-1]
#
# input_grid = img_processor.process_image(cutout)
# nn.calculatefitness(cutout)
