![UIC SVG Logo](https://uic.btmi.au/UIC.svg)

# Universal Instrumentation Code

The Universal Instrumentation Code or **UIC** was created by Peter Grimshaw of Zinfonia Holdings Pty Ltd in 2014 to create a standard for the identification of instrumental parts that is not bound to any language or convention and can provide new ways to search and display the instrumental requirements of printed music.

The UIC is the copyright of Zinfonia Holdings Pty Ltd and is licensed under a [Creative Commons Attribution 3.0 Unported License](https://creativecommons.org/licenses/by/3.0/) so that it can be freely used  by any sort of system that catalogues instrumental parts creating data sets that can be easily shared with different systems.

The files are updated every day at 01:00am (all dates and times are in UTC) and the status.xml/json file can be used to trigger any download updates.

A technical description of the UIC by Mark Carroll, James Koehne and Peter Grimshaw was published in the [Music Reference Services Quarterly Volume 17, 2014 – Issue 1](https://www.tandfonline.com/doi/abs/10.1080/10588167.2014.873226?mobileUi=0&journalCode=wmus20)

## XML Files

[https://uic.btmi.au/status.xml](https://uic.btmi.au/status.xml) - the date/time of last UIC update (LastUpdate) and the date/time the repositories were last checked (LastChecked)

[https://uic.btmi.au/domain.xml](https://uic.btmi.au/domain.xml) - a complete collection of UIC Domains

[https://uic.btmi.au/genus.xml](https://uic.btmi.au/genus.xml) - a complete collection of UIC Genera

[https://uic.btmi.au/instrument.xml](https://uic.btmi.au/instrument.xml) - a complete collection of UIC Instruments

[https://uic.btmi.au/descriptor.xml](https://uic.btmi.au/descriptor.xml) - a complete collection of UIC Descriptors

[https://uic.btmi.au/named.xml](https://uic.btmi.au/named.xml) - a complete collection of UIC Named Groups

[https://uic.btmi.au/icons.xml](https://uic.btmi.au/icons.xml) - a complete collection of UIC Icons

[https://uic.btmi.au/errata.xml](https://uic.btmi.au/errata.xml) - a complete collection of UIC Errata items

## JSON Files

[https://uic.btmi.au/status.json](https://uic.btmi.au/status.json) - the date/time of last UIC update (LastUpdate) and the date/time the repositories were last checked (LastChecked)

[https://uic.btmi.au/domain.json](https://uic.btmi.au/domain.json) - a complete collection of UIC Domains

[https://uic.btmi.au/genus.json](https://uic.btmi.au/genus.json) - a complete collection of UIC Genera

[https://uic.btmi.au/instrument.json](https://uic.btmi.au/instrument.json) - a complete collection of UIC Instruments

[https://uic.btmi.au/descriptor.json](https://uic.btmi.au/descriptor.json) - a complete collection of UIC Descriptors

[https://uic.btmi.au/named.json](https://uic.btmi.au/named.json) - a complete collection of UIC Named Groups

[https://uic.btmi.au/icons.json](https://uic.btmi.au/icons.json) - a complete collection of UIC Icons

[https://uic.btmi.au/errata.json](https://uic.btmi.au/errata.json) - a complete collection of UIC Errata items

## UIC Creation

UIC Values are 64-bit unsigned integers created by combining these values using the following algorithm:

* InstrumentID + (# << 32) + (DescriptorID << 37) + (DomainID << 51)

*# is the number of the instrument between 1 and 31, ie Violin 2*

Additional flags can also be added to the value to indicate: 

* Doubling: 80000000H (2147483648)
* Descriptor on left side 4000000000000H (1125899906842624)
* No physical part 8000000000000000H (-9223372036854775808)

For example, the following values for *[Solo] (Optional) Flute 1*:

* Instrument ID: 131083 Flute
* Domain ID: 2 Solo
* Descriptor ID: 80 (Optional)
* #: 1
* Left Align

would be calculated like this:

131083 + (1 << 32) + (80 << 37) + (2 << 51) + (1125899906842624) = **5640498945589259**

For your convenience we have included a C# Class [https://uic.btmi.au/UIC.cs](https://uic.btmi.au/UIC.cs) with many operations designed to assist in the creation and manipulation of UICs

Due to the limitations of some environments which are unable to manage large integers, an alternative syntax for the storage of UICs is sometimes used where the high and low 32-bit values are converted to hexadecimal and separated by a semicolon.  For example the UIC above would be represented as **140A01:2000B**

In the situation where more than one instrument is required for a part, each UIC can have up to 9 additional sub UIC values for each instrument. The Doubling flag is used to indicate if the same player plays all instruments

The UIC specification requires that the first UIC **must be unique** in any list and this UIC is also used to establish the order of the list

UICs are usually saved and shared as strings with each instrument on a separate line, and additional instruments or doubling instruments included on the same line separated by a space. The end of each line can also indicate a quantity of the same instrument with an equals sign

The following example is for a standard string quartet where each player doubles on kazoos

* 1:2000E 0:80040075=1
* 2:2000E 0:80040075=1
* 0:20016 0:80040075=1
* 0:2001E 0:80040075=1

which would translate as

* 1 x Violin 1/Kazoo
* 1 x Violin 2/Kazoo
* 1 x Viola/Kazoo
* 1 x Cello/Kazoo

This list was created using the UIC playground available here: [https://www.incopyright.com/#/uic](https://www.incopyright.com/#/uic) which you can use to test your implementation

## UIC Instrument IDs

Instrument IDs are structured in a hierarchical format to provide more information about the instrument itself and to allow for enhanced search techniques.

If you were to look at each individual bit of an Instrument ID (which is 31 bytes long), it looks like this:

| INSTRUMENT     | GENUS          | FAMILY |
| -------------- | -------------- | ------ |
| 00000000000000 | 00000000000000 | 000    |

Family is represented by these values:

* 001 (1): Scores
* 010 (2): Vocal
* 011 (3): Wind
* 100 (4): Brass
* 101 (5): Other
* 110 (6): Strings
* 111 (7): Footnotes

The Genus values (which are available for download) group instruments of a similar nature together and the Instrument is added incrementally for each new UIC.

For example, the UIC for Flute (131083) in binary looks like this:

**00000000000001 00000000000001 011**

And for Piccolo (262155) looks like this:

**00000000000010 00000000000001 011**

So you can see that bitwise searches for just the Genre and Family parts of the instrument allow you to locate works for either flute OR piccolo.
  
## UIC Footnotes

A special type of UIC has a family type of 7, and allows you to prepend or append special characters to denote footnotes where individual instruments need more information. Footnotes cannot be used as the primary UIC value, but when included in the list of UICs will display as a special symbol repeated before the name (or after the name if the Left Align flag is included).

Footnotes are constructed programmatically based on the values (the calculation can be found in the *GetInstNameFromUICValues* function in the UIC.CS document).
  
For example, a UIC with the values

  131083 131879

  would be displayed as 
  
  *Flute

The UIC specification allows for additional text to reference these footnotes if required by including the corresponding footnote marker at the start of each line.

## UIC Exchange Format

When saving or reading a group of UICs, the first line would be informational (about the source of the UIC), followed by each instrument on a separate line.  Doubling instruments are separated by w spce, and an indication of quantity may be appended at the end of the line after an equals sign (Note: the Quantity value is always an integer). Optional footnotes may also be appended after the UIC list is complete.

Here are two examples of the SAME UIC extracted from Zinfonia with a footnote included

**Hexadecimal format**
> Zinfonia UIC X12345
> 
> 1:2000E 0:80040075 0:20327=1
> 
> 2:2000E 0:80040075 0:20327=1
> 
> 0:20016 0:80040075 0:20327=1
> 
> 0:2001E 0:80040075 0:20327=1
> 
> \*Comb can be used instead of Kazoo

**Integer format**
> Zinfonia UIC X12345
> 
> 4295098382 2147745909 131879=1
> 
> 8590065678 2147745909 131879=1
> 
> 131094 2147745909 131879=1
> 
> 131102 2147745909 131879=1
> 
> \*Comb can be used instead of Kazoo


### for more information

[https://www.btmi.au](https://www.btmi.au)
