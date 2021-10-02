namespace SfmlProject.Geometry.Base {
    interface ICollidesWithPolygon {
        bool Collides(Polygon otherPolygon);
    }
}
