<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:pa="http://univ-grenoble-alpes/fr/l3miage/medical/patient"
                xmlns:cb="http://univ-grenoble-alpes/fr/l3miage/medical"
                xmlns:at="http://univ-grenoble-alpes/fr/l3miage/medical/acte"
>
    <xsl:output method="xml" indent="yes" />
    
    <xsl:param name="destinedName">Pien</xsl:param>

    <xsl:variable name="ngap" select="document('../xml/acte.xml', /)/at:ngap" />

    <xsl:variable name="patient" select="(//cb:patient[cb:nom=$destinedName])[1]" />
    
    <xsl:template match="/">
        <xsl:text>&#10;</xsl:text>
        <pa:patient 
            xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
            xmlns:cb="http://univ-grenoble-alpes/fr/l3miage/medical"
            xmlns:pa="http://univ-grenoble-alpes/fr/l3miage/medical/patient"
            xsi:schemaLocation="http://univ-grenoble-alpes/fr/l3miage/medical/patient ../xsd/patient.xsd"
        >
            <pa:nom><xsl:value-of select="$patient/cb:nom" /></pa:nom>
            <pa:prenom><xsl:value-of select="$patient/cb:prenom" /></pa:prenom>
            <pa:sexe><xsl:value-of select="$patient/cb:sexe" /></pa:sexe>
            <pa:naissance><xsl:value-of select="$patient/cb:naissance" /></pa:naissance>
            <pa:numeroSS><xsl:value-of select="$patient/cb:numero" /></pa:numeroSS>
            <pa:adresse>
                <cb:rue><xsl:value-of select="$patient/cb:adresse/cb:rue" /></cb:rue>
                <cb:codePostal><xsl:value-of select="$patient/cb:adresse/cb:codePostal" /></cb:codePostal>
                <cb:ville><xsl:value-of select="$patient/cb:adresse/cb:ville" /></cb:ville>
            </pa:adresse>
            <xsl:for-each select="$patient/cb:visite">
                <xsl:variable name="infirmier" select="//cb:infirmier[@id=current()/@intervenant]" />
                
                <xsl:element name="pa:visite">
                    <xsl:attribute name="date">
                        <xsl:value-of select="@date" />
                    </xsl:attribute>

                    <pa:intervenant>
                        <pa:nom><xsl:value-of select="$infirmier/cb:nom" /></pa:nom>
                        <pa:prenom><xsl:value-of select="$infirmier/cb:prenom" /></pa:prenom>
                    </pa:intervenant>

                    <xsl:for-each select="cb:acte">
                        <pa:acte><xsl:value-of select="$ngap/at:actes/at:acte[@id=current()/@id]" /></pa:acte>
                    </xsl:for-each>
                </xsl:element>
            </xsl:for-each>
        </pa:patient>
    </xsl:template>
    
</xsl:stylesheet>