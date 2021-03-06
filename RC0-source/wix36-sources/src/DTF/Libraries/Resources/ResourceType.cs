﻿//---------------------------------------------------------------------
// <copyright file="ResourceType.cs" company="Microsoft">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
//    
//    The use and distribution terms for this software are covered by the
//    Common Public License 1.0 (http://opensource.org/licenses/cpl1.0.php)
//    which can be found in the file CPL.TXT at the root of this distribution.
//    By using this software in any fashion, you are agreeing to be bound by
//    the terms of this license.
//    
//    You must not remove this notice, or any other, from this software.
// </copyright>
// <summary>
// Part of the Deployment Tools Foundation project.
// </summary>
//---------------------------------------------------------------------

namespace Microsoft.Deployment.Resources
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// Represents either a standard integer resource type or a custom resource type name.
    /// </summary>
    public class ResourceType
    {
        // Silence warnings about doc-comments
        #pragma warning disable 1591

        public static ResourceType None          { get { return "#0"; } }
        public static ResourceType Cursor        { get { return "#1"; } }
        public static ResourceType Bitmap        { get { return "#2"; } }
        public static ResourceType Icon          { get { return "#3"; } }
        public static ResourceType Menu          { get { return "#4"; } }
        public static ResourceType Dialog        { get { return "#5"; } }
        public static ResourceType String        { get { return "#6"; } }
        public static ResourceType FontDir       { get { return "#7"; } }
        public static ResourceType Font          { get { return "#8"; } }
        public static ResourceType Accelerator   { get { return "#9"; } }
        public static ResourceType RCData        { get { return "#10"; } }
        public static ResourceType MessageTable  { get { return "#11"; } }
        public static ResourceType GroupCursor   { get { return "#12"; } }
        public static ResourceType GroupIcon     { get { return "#14"; } }
        public static ResourceType Version       { get { return "#16"; } }
        public static ResourceType DialogInclude { get { return "#17"; } }
        public static ResourceType PlugPlay      { get { return "#19"; } }
        public static ResourceType Vxd           { get { return "#20"; } }
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ani")]
        public static ResourceType AniCursor     { get { return "#21"; } }
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ani")]
        public static ResourceType AniIcon       { get { return "#22"; } }
        public static ResourceType Html          { get { return "#23"; } }
        public static ResourceType Manifest      { get { return "#24"; } }

        #pragma warning restore 1591

        private string resourceType;

        /// <summary>
        /// Creates a new resource type from a string resource name.
        /// </summary>
        /// <param name="resourceType">String resource name,
        /// or an integer resource type prefixed by a #.</param>
        public ResourceType(string resourceType)
        {
            if (string.IsNullOrEmpty(resourceType))
            {
                throw new ArgumentNullException("resourceType");
            }

            this.resourceType = resourceType;

            if (this.IsInteger && this.IntegerValue < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid integer resource type value.");
            }
        }

        /// <summary>
        /// Creates a new integer resource type.
        /// </summary>
        /// <param name="resourceType">Integer value of a well-known resource type.</param>
        public ResourceType(int resourceType)
            : this("#" + resourceType)
        {
        }

        /// <summary>
        /// Gets a flag indicating whether the resource type is an integer type.
        /// </summary>
        public bool IsInteger
        {
            get
            {
                return this.resourceType.StartsWith("#", StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Gets the integer value of the resource type, or -1 if the resource type is not an integer.
        /// </summary>
        public int IntegerValue
        {
            get
            {
                int value;
                if (!this.IsInteger ||
                    !Int32.TryParse(this.resourceType.Substring(1), out value))
                {
                    value = -1;
                }

                return value;
            }
        }

        /// <summary>
        /// Gets a string representation of the resource type.
        /// </summary>
        /// <returns>The custom resource name, or the name of a well-known resource type.</returns>
        public override string ToString()
        {
            if (this.IsInteger)
            {
                switch (this.IntegerValue)
                {
                    case  0: return "None";
                    case  1: return "Cursor";
                    case  2: return "Bitmap";
                    case  3: return "Icon";
                    case  4: return "Menu";
                    case  5: return "Dialog";
                    case  6: return "String";
                    case  7: return "FontDir";
                    case  8: return "Font";
                    case  9: return "Accelerator";
                    case 10: return "RCData";
                    case 11: return "MessageTable";
                    case 12: return "GroupCursor";
                    case 14: return "GroupIcon";
                    case 16: return "Version";
                    case 17: return "DialogInclude";
                    case 19: return "PlugPlay";
                    case 20: return "Vxd";
                    case 21: return "AniCursor";
                    case 22: return "AniIcon";
                    case 23: return "Html";
                    case 24: return "Manifest";
                }
            }

            return this.resourceType;
        }

        /// <summary>
        /// Tests whether one resource type equals another object.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>True if equal, else false.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as ResourceType);
        }

        /// <summary>
        /// Tests whether one resource type equals another.
        /// </summary>
        /// <param name="otherType">Other resource type.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(ResourceType otherType)
        {
            return otherType != null && this.resourceType.Equals(otherType.resourceType, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets a hash code suitable for using the resource type as a dictionary key.
        /// </summary>
        /// <returns>Hash code based on the resource type string.</returns>
        public override int GetHashCode()
        {
            return this.resourceType.GetHashCode();
        }

        /// <summary>
        /// Implicitly converts a string to a ResourceType.
        /// </summary>
        /// <param name="resourceType">String resource type to convert.</param>
        /// <returns>ResourceType object.</returns>
        public static implicit operator ResourceType(string resourceType)
        {
            return new ResourceType(resourceType);
        }

        /// <summary>
        /// Explicitly converts a ResourceType to a string.
        /// </summary>
        /// <param name="resourceType">ResourceType object to convert.</param>
        /// <returns>The resource type string.</returns>
        /// <remarks>
        /// Unlike <see cref="ToString" />, this conversion does not return
        /// the common name of well-known integer resource types. Therefore,
        /// the returned string is suitable for passing directly to Win32
        /// resource APIs that accept resource type strings.
        /// </remarks>
        public static explicit operator string(ResourceType resourceType)
        {
            return resourceType.resourceType;
        }
    }
}

