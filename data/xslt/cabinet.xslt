<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:cb="http://univ-grenoble-alpes/fr/l3miage/medical"
    xmlns:at="http://univ-grenoble-alpes/fr/l3miage/medical/acte"
>
    <xsl:output method="html" indent="yes" />

    <xsl:param name="destinedId">002</xsl:param>
    
    <xsl:variable name="ngap" select="document('../xml/acte.xml', /)/at:ngap" />
    
    <xsl:template match="/">
        <html lang="fr-FR">
            <head>
                <title>Le Cabinet Infirmier</title>
                <meta name="description" content="" />
                <meta name="author" content="Marius ADJAKOTAN" />
                <meta name="author" content="Nadjidatie HOUSSOUNY" />
                <link rel="stylesheet" href="../css/cabinet.css" />
                <script src="../js/facture.js" defer="defer"></script>
            </head>
            
            <body>
                <header>
                    <h1>Mon Cabinet Infirmier</h1>
                </header>
                
                <main>
                    <xsl:apply-templates select="//cb:infirmier[@id=$destinedId]" />
                </main>
                
                <footer>
                    
                </footer>
            </body>
        </html>
    </xsl:template>
    
    <xsl:template match="//cb:infirmier[@id=$destinedId]">
        <xsl:variable name="id" select="./@id" />
        <div class="nursePatients">
            <p class="welcomeMessage">Bonjour <xsl:value-of select="cb:prenom" /> <xsl:text> </xsl:text> <xsl:value-of select="cb:nom" /></p>
            <p>Aujourd'hui, vous avez <xsl:value-of select="count(//cb:patient/cb:visite[@intervenant=$id])" /> patients.</p>
            <div class="listOfPatients">
                <xsl:call-template name="displayPatientsInfo">
                    <xsl:with-param name="patients" select="//cb:patient[cb:visite[@intervenant=$id]]" />
                </xsl:call-template>
            </div>
        </div>
    </xsl:template>
    
    <xsl:template name="displayPatientsInfo">
        <xsl:param name="patients" />
        <table class="tableOfPatients">
            <tr>
                <th>Nom</th>
                <th>Prénom(s)</th>
                <th>Adresse du patient</th>
                <th>Soins à effectuer</th>
            </tr>
            <xsl:for-each select="$patients">
                <tr class="line">
                    <td class="cell"><xsl:value-of select="cb:nom" /></td>
                    <td class="cell"><xsl:value-of select="cb:prenom" /></td>
                    <td class="cell"><xsl:value-of select="cb:adresse/cb:rue" />, <xsl:value-of select="cb:adresse/cb:codePostal" />, <xsl:value-of select="cb:adresse/cb:ville" /></td>
                    <td class="cell">
                        <ul>
                            <xsl:for-each select="cb:visite/cb:acte">
                                <li><xsl:value-of select="$ngap/at:actes/at:acte[@id=current()/@id]" /></li>
                            </xsl:for-each>
                        </ul>
                    </td>
                    <td class="cell">
                        <xsl:element name="button">
                            <xsl:attribute name="class">showInvoice</xsl:attribute>
                            <xsl:attribute name="onclick">
                                openInvoice('<xsl:value-of select="cb:prenom"/>',
                                            '<xsl:value-of select="cb:nom"/>',
                                            '<xsl:value-of select="cb:visite/cb:acte/@id"/>'
                                )
                            </xsl:attribute>
                            Facture
                        </xsl:element>
                    </td>
                </tr>
            </xsl:for-each>
        </table>
    </xsl:template>
    
</xsl:stylesheet>
    