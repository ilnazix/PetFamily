﻿using System.Reflection;

namespace PetFamily.VolunteerRequest.Contracts;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly; 
}
