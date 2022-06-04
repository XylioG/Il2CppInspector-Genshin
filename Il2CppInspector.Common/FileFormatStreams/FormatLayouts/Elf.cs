﻿/*
    Copyright 2017 Perfare - https://github.com/Perfare/Il2CppDumper
    Copyright 2017-2021 Katy Coe - http://www.djkaty.com - https://github.com/djkaty

    All rights reserved.
*/

using System;
using NoisyCowStudios.Bin2Object;

namespace Il2CppInspector
{
    [Flags]
    public enum Elf : uint
    {
        // elf_header.m_dwFormat
        ELFMAG = 0x464c457f, // "\177ELF"

        // elf_header.e_machine
        EM_386 = 0x03,
        EM_ARM = 0x28,
        EM_X86_64 = 0x3E,
        EM_AARCH64 = 0xB7,

        // elf_header.m_arch
        ELFCLASS32 = 1,
        ELFCLASS64 = 2,

        // elf_header.e_Type
        ET_EXEC = 2,

        // PHTs
        PT_LOAD = 1,
        PT_DYNAMIC = 2,

        PF_X = 1,
        PF_W = 2,
        PF_R = 4,

        // SHTs
        SHT_PROGBITS = 1,
        SHT_SYMTAB = 2,
        SHT_STRTAB = 3,
        SHT_RELA = 4,
        SHT_NOBITS = 8,
        SHT_REL = 9,
        SHT_DYNSYM = 11,

        // SHNs
        SHN_UNDEF = 0,

        // STTs
        STT_NOTYPE = 0,
        STT_OBJECT = 1,
        STT_FUNC = 2,
        STT_SECTION = 3,
        STT_FILE = 4,
        STT_COMMON = 5,
        STT_LOOS = 10,
        STT_HIOS = 12,
        STT_LOPROC = 13,
        STT_SPARC_REGISTER = 13,
        STT_HIPROC = 15,

        // SHFs
        SHF_ALLOC = 2,
        SHF_EXECINSTR = 4,

        // dynamic sections
        DT_PLTGOT = 3,
        DT_HASH = 4,
        DT_STRTAB = 5,
        DT_SYMTAB = 6,
        DT_RELA = 7,
        DT_RELASZ = 8,
        DT_RELAENT = 9,
        DT_INIT = 12,
        DT_FINI = 13,
        DT_REL = 17,
        DT_RELSZ = 18,
        DT_RELENT = 19,
        DT_JMPREL = 23,
        DT_INIT_ARRAY = 25,
        DT_FINI_ARRAY = 26,
        DT_INIT_ARRAYSZ = 27,
        DT_PREINIT_ARRAY = 32,
        DT_MOVETAB = 0x6ffffefe,
        DT_VERDEF = 0x6ffffffc,
        DT_VERNEED = 0x6ffffffe,
        DT_SYMINFO = 0x6ffffeff,

        // relocation types
        R_ARM_ABS32 = 2,
        R_ARM_REL32 = 3,
        R_ARM_COPY = 20,

        R_AARCH64_ABS64 = 0x101,
        R_AARCH64_PREL64 = 0x104,
        R_AARCH64_GLOB_DAT = 0x401,
        R_AARCH64_JUMP_SLOT = 0x402,
        R_AARCH64_RELATIVE = 0x403,

        R_386_32 = 1,
        R_386_PC32 = 2,
        R_386_GLOB_DAT = 6,
        R_386_JMP_SLOT = 7,

        R_AMD64_64 = 1
    }

#pragma warning disable CS0649
    public class elf_header<TWord> where TWord : struct
    {
        // 0x7f followed by ELF in ascii
        public uint m_dwFormat;

        // 1 - 32 bit
        // 2 - 64 bit
        public byte m_arch;

        // 1 - little endian
        // 2 - big endian
        public byte m_endian;

        // 1 is original elf format
        public byte m_version;

        // set based on OS, refer to OSABI enum
        public byte m_osabi;

        // refer to elf documentation
        public byte m_osabi_ver;

        // unused
        [ArrayLength(FixedSize=7)]
        public byte[] e_pad;//byte[7]

        // 1 - relocatable
        // 2 - executable
        // 3 - shared
        // 4 - core
        public ushort e_type;

        // refer to isa enum
        public ushort e_machine;

        public uint e_version;

        public TWord e_entry;
        public TWord e_phoff;
        public TWord e_shoff;
        public uint e_flags;
        public ushort e_ehsize;
        public ushort e_phentsize;
        public ushort e_phnum;
        public ushort e_shentsize;
        public ushort e_shnum;
        public ushort e_shtrndx;
    }

    public interface Ielf_phdr<TWord> where TWord : struct
    {
        uint p_type { get; }
        TWord p_offset { get; set; }
        TWord p_filesz { get; set; }
        TWord p_memsz { get; }
        TWord p_vaddr { get; set; }
        uint p_flags { get; }
    }

    public class elf_32_phdr : Ielf_phdr<uint> {
        public uint p_type => f_p_type;
        public uint p_offset { get => f_p_offset; set => f_p_offset = value; }
        public uint p_filesz { get => f_p_filesz; set => f_p_filesz = value; }
        public uint p_vaddr { get => f_p_vaddr; set => f_p_vaddr = value; }
        public uint p_flags => f_p_flags;
        public uint p_memsz => f_p_memsz;

        public uint f_p_type;
        public uint f_p_offset;
        public uint f_p_vaddr;
        public uint p_paddr;
        public uint f_p_filesz;
        public uint f_p_memsz;
        public uint f_p_flags;
        public uint p_align;
    }

    public class elf_64_phdr : Ielf_phdr<ulong>
    {
        public uint p_type => f_p_type;
        public ulong p_offset { get => f_p_offset; set => f_p_offset = value; }
        public ulong p_filesz { get => f_p_filesz; set => f_p_filesz = value; }
        public ulong p_memsz => f_p_memsz;
        public ulong p_vaddr { get => f_p_vaddr; set => f_p_vaddr = value; }
        public uint p_flags => f_p_flags;

        public uint f_p_type;
        public uint f_p_flags;
        public ulong f_p_offset;
        public ulong f_p_vaddr;
        public ulong p_paddr;
        public ulong f_p_filesz;
        public ulong f_p_memsz;
        public ulong p_align;
    }

    public class elf_shdr<TWord> where TWord : struct
    {
        public uint sh_name;
        public uint sh_type;
        public TWord sh_flags;
        public TWord sh_addr;
        public TWord sh_offset;
        public TWord sh_size;
        public uint sh_link;
        public uint sh_info;
        public TWord sh_addralign;
        public TWord sh_entsize;
    }

    public interface Ielf_sym<TWord> where TWord : struct
    {
        uint st_name { get; }
        TWord st_value { get; }
        ushort st_shndx { get; }
        Elf st_info { get; }
        Elf type { get; }
    }

    public class elf_32_sym : Ielf_sym<uint>
    {
        public uint st_name => f_st_name;
        public uint st_value => f_st_value;
        public ushort st_shndx => f_st_shndx;
        public Elf st_info => (Elf) f_st_info;
        public Elf type => (Elf) (f_st_info & 0xf);

        public uint f_st_name;
        public uint f_st_value;
        public uint st_size;
        public byte f_st_info;
        public byte st_other;
        public ushort f_st_shndx;
    }

    public class elf_64_sym : Ielf_sym<ulong>
    {
        public uint st_name => f_st_name;
        public ulong st_value => f_st_value;
        public ushort st_shndx => f_st_shndx;
        public Elf st_info => (Elf) f_st_info;
        public Elf type => (Elf) (f_st_info & 0xf);

        public uint f_st_name;
        public byte f_st_info;
        public byte st_other;
        public ushort f_st_shndx;
        public ulong f_st_value;
        public ulong st_size;
    }

    public class elf_dynamic<TWord> where TWord : struct
    {
        public TWord d_tag;
        public TWord d_un;
    }

    public class elf_rel<TWord> where TWord : struct
    {
        public TWord r_offset;
        public TWord r_info;
    }

    public class elf_rela<TWord> where TWord : struct
    {
        public TWord r_offset;
        public TWord r_info;
        public TWord r_addend;
    }
#pragma warning restore CS0649
}
