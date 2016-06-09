# StraightSkeletonNet

Implementation of straight skeleton algorithm for polygons with holes. It is based on concept of tracking bisector intersection with queue of events to process and circular list with processed events called lavs. This implementation is highly modified concept described by Petr Felkel and Stepan Obdrzalek. In compare to original this algorithm has new kind of event and support for multiple events which appear in the same distance from edges. It is common when processing degenerate cases caused by polygon with right angles.

This port has no external dependencies: all needed classes are ported. Original Java code depends on external libraries which provide some primitives: vector, line, ray, etc. It can be found here:
https://github.com/kendzi/kendzi-math/tree/master/kendzi-straight-skeleton

You can read more about this implementation at blog: 
http://reinterpretcat.blogspot.de/2016/02/straight-skeleton-on-net.html
