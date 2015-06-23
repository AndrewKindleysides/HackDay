using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Models;

namespace App
{
    public class Repo
    {
       private readonly List<Member> _members;

        public Repo()
        {
            _members= new List<Member>();
        }

        public void AddMember(Member member)
        {
            _members.Add(member);
        }

        public List<Member> AllMembers()
        {
            return _members;
        }

    }
}