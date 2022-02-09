using System.Collections.Generic;
using BilliardClub.Models;

namespace BilliardClub.View_Models
{
    public class TypeTableAndRotationViewModel
    {
        public List<TypeTable> typeTables { get; set; }
        public List<TableRotation> tableRotations { get; set; }
    }
}