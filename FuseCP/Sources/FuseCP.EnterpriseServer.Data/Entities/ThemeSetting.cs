// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data.Entities;

#if NetCore
//[Keyless]
[Index("ThemeId", Name = "ThemeSettingsIdx_ThemeID")]
#endif
public partial class ThemeSetting
{
    [Key]
    [Column("ThemeSettingID")]
    public int ThemeSettingId { get; set; }

    [Column("ThemeID")]
    public int ThemeId { get; set; }

    [Required]
    [StringLength(255)]
    public string SettingsName { get; set; }

    [Required]
    [StringLength(255)]
    public string PropertyName { get; set; }

    [Required]
    [StringLength(255)]
    public string PropertyValue { get; set; }
}
#endif
