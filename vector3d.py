# Created: 15.2.2011
# Version: 0.2
# Author: Dolkar
# License: None


import math

_BASIS_NAMES = ['i', 'j', 'k']


class DiffDimError(Exception):
    def __str__(self):
        return 'Only vectors with same dimensions are needed'


class Vector(object):
    '''
    2D/3D Vector class allowing basic vector operations.

    Vector(x,y[,z]) or Vector(tuple) - more efficient
    If no argument is passed, an empty 2D vector is created.

    Usage:
    >>> from math import degrees, radians
    >>> v = Vector(8, 6)
    >>> v
    <2D Vector object: x = 8, y = 6>
    >>> v.x
    8.0
    >>> v.y = 4
    >>> v[0:2]
    [8.0, 4.0]
    >>> v.mag()
    8.94427190999916
    >>> v.normSelf()   # Use normSelf() instead of v = v.norm()
    >>> v.isUnit()
    True
    >>> degrees(v.direction())
    26.56505117707799
    >>> w = Vector.polar(angle = radians(45), mag = 2)
    >>> w
    <2D Vector object: x = 1.41421, y = 1.41421>
    >>> degrees(v.angle(w))
    18.434948822922017
    >>> v.rotate(v.angle(w))
    <2D Vector object: x = 6.32456, y = 6.32456>
    >>> v.dot(w)
    16.97056274847714
    >>> v.project(w)
    <2D Vector object: x = 1.6970563, y = 0.84852814>
    >>> -v.reflect(w.norm())  # Negate the result to reflect it off the surface
    <2D Vector object: x = 4, y = 8>
    >>> v.z = 5
    >>> v.is3D()
    True
    >>> w = [0, 8, 0]
    >>> v.cross(w)
    <3D Vector object: x = -40, y = 0, z = 64>
    >>> tuple(v)
    (8.0, 4.0, 5.0)
    >>> v.stdBasis()
    '8i + 4j + 5k'
    '''

    def __init__(self, *args):
        try:
            self._val = [float(item) for item in args[0]]
        except TypeError:
            self._val = [float(item) for item in args]
        except IndexError:
            self._val = [0.0, 0.0]

        self._len = len(self._val)
        if not 2 <= self._len <= 3:
            raise TypeError('Only two or three dimensional vectors accepted')

    def __len__(self):
        return self._len

    def __eq__(self, other):
        return self._val[:] == other[:]

    def __ne__(self, other):
        return not self.__eq__(other)

    def __neg__(self):
        return Vector([-item for item in self._val])

    def __add__(self, other):
        if self._len != len(other):
            raise DiffDimError

        return Vector([self._val[i] + other[i] for i in range(self._len)])

    __radd__ = __add__

    def __iadd__(self, other):
        if self._len != len(other):
            raise DiffDimError

        self._val = [self._val[i] + other[i] for i in range(self._len)]
        return self

    def __sub__(self, other):
        if self._len != len(other):
            raise DiffDimError

        return Vector([self._val[i] - other[i] for i in range(self._len)])

    def __rsub__(self, other):
        if self._len != len(other):
            raise DiffDimError

        return Vector([other[i] - self._val[i] for i in range(self._len)])

    def __isub__(self, other):
        if self._len != len(other):
            raise DiffDimError

        self._val = [self._val[i] - other[i] for i in range(self._len)]
        return self

    def __mul__(self, scalar):
        return Vector([item * scalar for item in self._val])

    def __imul__(self, scalar):
        self._val = [item * scalar for item in self._val]
        return self

    def __div__(self, scalar):
        return Vector([item / scalar for item in self._val])

    def __idiv__(self, scalar):
        self._val = [item / scalar for item in self._val]
        return self

    def __pow__(self, scalar):
        return Vector([item ** scalar for item in self._val])

    def __ipow__(self, scalar):
        self._val = [item ** scalar for item in self._val]
        return self

    def __abs__(self):
        return Vector([abs(item) for item in self._val])

    def __nonzero__(self):
        return any(self._val)

    def __repr__(self):
        if self.is3D():
            out = '<3D Vector object: x = {:.6g}, y = {:.6g}, z = {:.6g}>'
        else:
            out = '<2D Vector object: x = {:.6g}, y = {:.6g}>'

        return out.format(*self._val)

    def __getitem__(self, key):
        return self._val[key]

    def __setitem__(self, key, value):
        self._val[key] = float(value)

    def __iter__(self):
        return iter(self._val)

    def _rotatePlane(self, axis_1, axis_2, q):
        '''Helper function rotating a plane by q'''
        sin_q = math.sin(q)
        cos_q = math.cos(q)

        res_1 = axis_1 * cos_q - axis_2 * sin_q
        res_2 = axis_1 * sin_q + axis_2 * cos_q

        return res_1, res_2

    @classmethod
    def polar(cls, angle, mag):
        '''Returns 2D vector from given angle in radians and magnitude'''
        return Vector((mag * math.cos(angle), mag * math.sin(angle)))

    def is3D(self):
        '''Returns True if vector has three dimensions'''
        return self._len - 2

    def isUnit(self):
        '''Returns True if Vector is an unit Vector, magnitude equals to 1'''
        return round(self.mag(), 10) == 1.0

    def copy(self):
        '''Returns copy of the vector'''
        return Vector(self._val)

    def clear(self):
        '''Sets vector to 0'''
        self._val = [0 for item in self._val]

    def stdBasis(self):
        '''Standard basis vector representation in the format xi + yj + zk'''
        res = []
        for i, val in enumerate(self._val):
            val = round(val, 3)
            if val == 1:
                res.append(_BASIS_NAMES[i])
            elif val != 0:
                res.append('{:g}{}'.format(val, _BASIS_NAMES[i]))

        return ' + '.join(res)

    def mag(self):
        '''Returns magnitude of the vector.'''
        return math.sqrt(sum([item ** 2 for item in self._val]))

    def direction(self):
        '''Returns the direction angle (2D) of the vector in radians.'''
        return math.atan2(self._val[1], self._val[0])

    def angle(self, other):
        '''Returns the angle between two vectors.'''
        if self._len != len(other):
            raise DiffDimError

        a = self.norm()
        b = other.norm()
        return math.acos(a.dot(b))

    def norm(self):
        '''Returns normalized vector (unit vector) of self.'''
        return self / self.mag()

    def normSelf(self):
        '''Normalize the vector.'''
        self /= self.mag()
        return self

    def reflect(self, normal):
        '''
        Returns the vector reflected in a normalized vector as a surface.
        You may want to negate the result to reflect the vector off a surface.
        '''
        if self._len != len(normal):
            raise DiffDimError

        dot = self.dot(normal) * 2
        result = [self._val[i] - dot * normal[i] for i in range(self._len)]
        return Vector(result)

    def reflectSelf(self, normal):
        '''
        Reflects the vector in a normalized vector as a surface.
        You may want to negate the result to reflect the vector off a surface.
        '''
        if self._len != len(normal):
            raise DiffDimError

        dot = self.dot(normal) * 2
        self._val = [self._val[i] - dot * normal[i] for i in range(self._len)]
        return self

    def rotate(self, z_angle=0, x_angle=0, y_angle=0):
        '''Returns a rotated vector around z, (x, y) axes.'''
        res = Vector(tuple(self))

        if z_angle:
            res[0], res[1] = self._rotatePlane(res[0], res[1], z_angle)

        if self.is3D():
            if x_angle:
                res[1], res[2] = self._rotatePlane(res[1], res[2], x_angle)
            if y_angle:
                res[2], res[0] = self._rotatePlane(res[2], res[0], y_angle)

        return res

    def rotateSelf(self, z_angle=0, x_angle=0, y_angle=0):
        '''Rotates the vector around z, (x, y) axes.'''
        val = self._val

        if z_angle:
            val[0], val[1] = self._rotatePlane(val[0], val[1], z_angle)

        if self.is3D():
            if x_angle:
                val[1], val[2] = self._rotatePlane(val[1], val[2], x_angle)
            if y_angle:
                val[2], val[0] = self._rotatePlane(val[2], val[0], y_angle)

        return self

    def dot(self, other):
        '''Returns the scalar dot product of two vectors.'''
        if self._len != len(other):
            raise DiffDimError

        return sum([self._val[i] * other[i] for i in range(self._len)])

    def cross(self, other):
        '''Returns the cross product of two vectors.'''
        a = self._val
        b = other
        try:
            return Vector((a[1] * b[2] - a[2] * b[1]),
                          (a[2] * b[0] - a[0] * b[2]),
                          (a[0] * b[1] - a[1] * b[0]))
        except IndexError:
            raise TypeError('Only three dimensional vectors can make a cross product.')

    def project(self, other):
        '''Returns the projection of another vector on self.'''
        scalar = self.dot(other) / self.mag() ** 2
        return self * scalar

    @property
    def x(self):
        return self._val[0]

    @x.setter
    def x(self, value):
        self._val[0] = float(value)

    @property
    def y(self):
        return self._val[1]

    @y.setter
    def y(self, value):
        self._val[1] = float(value)

    @property
    def z(self):
        try:
            return self._val[2]
        except IndexError:
            raise IndexError('Vector is two dimensional.')

    @z.setter
    def z(self, value):
        try:
            self._val[2] = float(value)
        except IndexError:
            self._val.append(float(value))
            self._len = 3

    @z.deleter
    def z(self):
        try:
            del(self._val[2])
            self._len = 2
        except IndexError:
            raise IndexError('Vector is two dimensional.')
