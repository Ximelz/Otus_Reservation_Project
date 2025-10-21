using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public static class PersonReserveMapper
    {
        public static PersonReserveModel MapToModel(this PersonReserve person) => new PersonReserveModel {
                                                                                  Id = person.Id,
                                                                                  Email = person.Email,
                                                                                  Phone = person.Phone,
                                                                                  Name = person.Name,
                                                                                  ContactType = person.ContactType };

        public static PersonReserve MapFromModel(this PersonReserveModel personModel) => new PersonReserve(
                                                                                         personModel.Id,
                                                                                         personModel.Email,
                                                                                         personModel.Phone,
                                                                                         personModel.Name,
                                                                                         personModel.ContactType );

        public static List<PersonReserve> MapListFromModel(this List<PersonReserveModel> personModelList)
        {
            var list = new List<PersonReserve>();
            foreach (var personModel in personModelList)
                list.Add(personModel.MapFromModel());
            return list;
        }

        public static List<PersonReserveModel> MapListToModel(this List<PersonReserve> personList)
        {
            var list = new List<PersonReserveModel>();
            foreach (var person in personList)
                list.Add(person.MapToModel());
            return list;
        }
    }
}
