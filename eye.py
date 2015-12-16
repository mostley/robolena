#!/usr/bin/python

import pygame, math
from vector3d import Vector


class Main:

    def __init__(self):
        pygame.init()

        self.screenDim = (500, 500)
        self.screen = pygame.display.set_mode(self.screenDim)
        self.background = (255, 255, 255)
        self.running = False

        self.dots = [
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0
        ]

        self.lookdirection = Vector(0.3, 1, 0.3)
        self.sphereRadius = 4

    def poll(self):
        events = pygame.event.get()
        for e in events:
            if e.type == pygame.QUIT:
                self.running = False
            elif e.type == pygame.KEYUP:
                if e.key == pygame.K_ESCAPE:
                    self.running = False

    def update(self, dt):
        direction = self.lookdirection
        posOnSphere = direction.norm() * self.sphereRadius

        pos = Vector(posOnSphere.x, posOnSphere.z) + Vector(3.5, 3.5)
        visible = self.lookdirection.y > 0

        for x in range(8):
            for y in range(8):
                index = x + 8 * y
                if visible:
                    distance = (Vector(x, y) - pos).mag()
                    self.dots[index] = distance
                else:
                    self.dots[index] = 999

        self.lookdirection.rotateSelf(0, 0, 0.1)

    def draw(self):
        step = self.screenDim[0] / 8
        margin = step/2
        radius = step/2 - 10
        for x in range(8):
            for y in range(8):
                pos = (x * step + margin, y * step + margin)
                index = x + 8 * y
                dot = self.dots[index]
                dist = self.sphereRadius - (Vector(x, y) - Vector(3.5, 3.5)).mag()
                color = 80 * (-(dist - 1)) if dist < 1 else 0

                eyeColor = (2 - dot) * 250
                color += eyeColor if eyeColor > 0 else 0

                color = color if color >= 0 else 0
                color = color if color < 255 else 255
                pygame.draw.circle(self.screen, (0, int(color), 0), pos, radius, 0)

    def run(self):

        self.running = True
        clock = pygame.time.Clock()
        while self.running:
            dt = clock.tick(40) / 1000.0

            self.screen.fill(self.background)

            self.poll()
            self.update(dt)
            self.draw()

            pygame.display.flip()

if __name__ == '__main__':
    main = Main()
    print "starting..."
    main.run()
    print "shuting down..."
