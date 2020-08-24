using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class YYInstance
    {
        public uint ID { get; private set; }
        public string ObjectName { get; private set; }
        public YYObject ObjectRef { get; private set; }

        public List<GMValue> BuiltinVariables { get; private set; }
        public List<GMValue> InstVariables { get; private set; }

        public void Serialize(BinaryReader reader)
        {
            ID = reader.ReadUInt32();
            ReadBuiltins(reader);
            uint custom_var_len = reader.ReadUInt32();
            InstVariables = new List<GMValue>((int)custom_var_len);
            for (int i = 0; i < custom_var_len; i++)
            {
                string _name = "";

            }
        }

        private void ReadBuiltins(BinaryReader reader)
        {
            BuiltinVariables = new List<GMValue>();
            AddBuiltin("object_index", reader.ReadUInt32());
            AddBuiltin("x", reader.ReadSingle());
            AddBuiltin("y", reader.ReadSingle());
            AddBuiltin("direction", reader.ReadSingle());
            AddBuiltin("friction", reader.ReadSingle());
            AddBuiltin("gravity", reader.ReadSingle());
            AddBuiltin("gravity_direction", reader.ReadSingle());
            AddBuiltin("hspeed", reader.ReadSingle());
            AddBuiltin("vspeed", reader.ReadSingle());
            AddBuiltin("speed", reader.ReadSingle());
            AddBuiltin("xprevious", reader.ReadSingle());
            AddBuiltin("yprevious", reader.ReadSingle());
            AddBuiltin("sprite_index", reader.ReadInt32());
            AddBuiltin("image_alpha", reader.ReadSingle());
            AddBuiltin("image_angle", reader.ReadSingle());
            AddBuiltin("image_blend", reader.ReadUInt32());
            AddBuiltin("image_index", reader.ReadSingle());
            AddBuiltin("image_number", reader.ReadInt32());
            AddBuiltin("image_speed", reader.ReadSingle());
            AddBuiltin("image_xscale", reader.ReadSingle());
            AddBuiltin("image_yscale", reader.ReadSingle());
            for (int i = 0; i < 12; i++) AddBuiltin($"alarm[{i}]", reader.ReadInt32());
            AddBuiltin("path_index", reader.ReadInt32());
            AddBuiltin("path_position", reader.ReadSingle());
            AddBuiltin("path_positionprevious", reader.ReadSingle());
            AddBuiltin("path_endaction", reader.ReadSingle());
            AddBuiltin("path_scale", reader.ReadSingle());
            AddBuiltin("path_speed", reader.ReadSingle());
            AddBuiltin("path_orientation", reader.ReadSingle());
            AddBuiltin("xstart", reader.ReadSingle());
            AddBuiltin("ystart", reader.ReadSingle());
            AddBuiltin("persistent", reader.ReadInt32() != 0);
            AddBuiltin("depth", reader.ReadSingle());
            AddBuiltin("visible", reader.ReadInt32() != 0);
            AddBuiltin("mask_index", reader.ReadInt32());
            AddBuiltin("solid", reader.ReadInt32() != 0);
            AddBuiltin("bbox_top", reader.ReadInt32());
            AddBuiltin("bbox_bottom", reader.ReadInt32());
            AddBuiltin("bbox_left", reader.ReadInt32());
            AddBuiltin("bbox_right", reader.ReadInt32());
            AddBuiltin("sprite_width", reader.ReadSingle());
            AddBuiltin("sprite_height", reader.ReadSingle());
            AddBuiltin("sprite_xoffset", reader.ReadSingle());
            AddBuiltin("sprite_yoffset", reader.ReadSingle());
            AddBuiltin("phy_active", reader.ReadInt32() != 0);
            AddBuiltin("phy_fixed_rotation", reader.ReadInt32() != 0);
            AddBuiltin("phy_angular_velocity", reader.ReadSingle());
            AddBuiltin("phy_linear_velocity_x", reader.ReadSingle());
            AddBuiltin("phy_linear_velocity_y", reader.ReadSingle());
            AddBuiltin("phy_speed_x", reader.ReadSingle());
            AddBuiltin("phy_speed_y", reader.ReadSingle());
            AddBuiltin("phy_position_x", reader.ReadSingle());
            AddBuiltin("phy_position_y", reader.ReadSingle());
            AddBuiltin("phy_rotation", reader.ReadSingle());
            AddBuiltin("phy_bullet", reader.ReadInt32() != 0);
            AddBuiltin("phy_com_x", reader.ReadSingle());
            AddBuiltin("phy_com_y", reader.ReadSingle());
            AddBuiltin("phy_dynamic", reader.ReadInt32() != 0);
            AddBuiltin("phy_kinematic", reader.ReadInt32() != 0);
            AddBuiltin("phy_inertia", reader.ReadSingle());
            AddBuiltin("phy_mass", reader.ReadSingle());
            AddBuiltin("phy_sleeping", reader.ReadInt32() != 0);
            AddBuiltin("timeline_index", reader.ReadInt32());
            AddBuiltin("timeline_running", reader.ReadInt32() != 0);
            AddBuiltin("timeline_speed", reader.ReadSingle());
            AddBuiltin("timeline_position", reader.ReadSingle());
            AddBuiltin("timeline_loop", reader.ReadInt32() != 0);
        }

        private void AddBuiltin(string var_name, bool value)
        {
            var __value = new GMValue
            {
                ValType = GMValue.GMValueType.Real,
                Number = value ? 1 : 0,
                Name = var_name
            };
            BuiltinVariables.Add(__value);
        }

        private void AddBuiltin(string var_name, float value)
        {
            var __value = new GMValue
            {
                ValType = GMValue.GMValueType.Real,
                Number = value,
                Name = var_name
            };
            BuiltinVariables.Add(__value);
        }

        public override string ToString()
        {
            return $"{ID} | {ObjectName}";
        }
    }
}
