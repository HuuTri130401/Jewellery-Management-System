using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.ResponseModels;

namespace THT.JMS.Application.SearchModels
{
    public class UserSearch : IRequest<List<UserModel>>
    {

    }
}
