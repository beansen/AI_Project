import cv2
import numpy as np


walkable = cv2.imread('images/walkable.png', 0)
marioImage = cv2.imread('images/marioFaceSmall.png', 0)
obstacles = []
grid = np.zeros((17, 15))
method = eval('cv2.TM_CCOEFF_NORMED')


def showimage(img):
	cv2.imshow('window', img)
	cv2.waitKey(0)
	cv2.destroyAllWindows()


def init():
	obstacles.append(cv2.imread('images/coopaFace.png', 0))
	obstacles.append(cv2.imread('images/pipeTop.png', 0))


def drawgrid(img):
	for x in range(0, grid.shape[0]):
		for y in range(0, grid.shape[1]):
			thickness = 1 if grid[x, y] == 0 else -1
			color = 255 if grid[x, y] == 0 else 0
			cv2.rectangle(img, (x*36, y*25), (x*36 + 36, y*25 + 25), color, thickness)

	showimage(img)


def __findcoopa(img):
	res = cv2.matchTemplate(img, obstacles[0], method)
	loc = np.where(res >= 0.8)
	for pt in zip(*loc[::-1]):
		gridX = pt[0] / 36 if pt[0] % 36 < 27 else pt[0] / 36 + 1
		gridY = (pt[1]) / 25
		grid[gridX, gridY] = 1
		grid[gridX, gridY + 1] = 2

	res = cv2.matchTemplate(img, cv2.flip(obstacles[0], 1), method)
	loc = np.where(res >= 0.8)
	for pt in zip(*loc[::-1]):
		gridX = pt[0] / 36 if pt[0] % 36 < 27 else pt[0] / 36 + 1
		gridY = (pt[1]) / 25
		grid[gridX, gridY] = 1
		grid[gridX, gridY + 1] = 2


def __findPipe(img):
	res = cv2.matchTemplate(img, obstacles[1], method)
	loc = np.where(res >= 0.9)
	for pt in zip(*loc[::-1]):
		gridX = pt[0] / 36 if pt[0] % 36 < 27 else pt[0] / 36 + 1
		gridY = (pt[1]) / 25
		grid[gridX, gridY] = 1
		grid[gridX, gridY + 1] = 2


def __findwalkable(img):
	res = cv2.matchTemplate(img, walkable, method)
	loc = np.where(res >= 0.8)
	for pt in zip(*loc[::-1]):
		gridX = pt[0] / 36
		gridY = (pt[1]) / 25
		grid[gridX, gridY + 1] = 1


def process_image(img):
	grid.fill(0)
	__findwalkable(img)
	__findcoopa(img)
	__findPipe(img)
	return grid
	# drawgrid(img)
