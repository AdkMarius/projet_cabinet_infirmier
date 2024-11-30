<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:pa="http://univ-grenoble-alpes/fr/l3miage/medical/patient"
                xmlns:cb="http://univ-grenoble-alpes/fr/l3miage/medical"
>
    <xsl:output method="html" indent="yes" />
    
    <xsl:variable name="smallcase" select="abcdefghijklmnopqrstuvwxyz" />
    <xsl:variable name="uppercase" select="ABCDEFGHIJKLMNOPQRSTUVWXYZ" />
    
    <xsl:template match="/">
        <html lang="fr-FR">
            <head>
                <title>La fiche Patient</title>
                <meta name="description" content="" />
                <meta name="author" content="Marius ADJAKOTAN" />
                <meta name="author" content="Nadjidatie HOUSSOUNY" />
                <link rel="stylesheet" href="../css/cabinet.css" />
            </head>

            <body>
                <header>
                    <h1>Mon Cabinet Infirmier</h1>
                </header>

                <main>
                    <div>
                        <h2>Bonjour <xsl:text disable-output-escaping="yes" />
                            <xsl:choose>
                                <xsl:when test="//pa:sexe='M'">Monsieur </xsl:when>
                                <xsl:otherwise>Madame </xsl:otherwise>
                            </xsl:choose>
                            <xsl:value-of select="translate(//pa:nom, $smallcase, $uppercase)" /> <xsl:text> </xsl:text> <xsl:value-of select="//pa:prenom" />
                        </h2>
                        <p>Voici vos différentes informations : </p>
                        <ul>
                            <li>Numéro de sécurité sociale est : <xsl:value-of select="//pa:numeroSS" /></li>
                            <li>Date de naissance : <xsl:value-of select="//pa:naissance" /></li>
                            <li>Adresse : <xsl:value-of select="//pa:adresse/cb:rue" />, <xsl:value-of select="//pa:adresse/cb:codePostal" />, <xsl:value-of select="//pa:adresse/cb:ville" /></li>
                        </ul>
                    </div>
                    
                    <div>
                        <p>Voici les différentes informations concernant vos visites : </p>
                        <table>
                            <tr>
                                <th>Date</th>
                                <th>Actes Médicaux</th>
                                <th>Intervenant</th>
                            </tr>
                            <xsl:for-each select="//pa:visite">
                                <xsl:sort select="@date" />
                                <tr>
                                    <td class="patientTableCell"><xsl:value-of select="@date" /></td>
                                    <td>
                                        <ul>
                                            <xsl:apply-templates select="pa:acte" />
                                        </ul>
                                    </td>
                                    <td class="patientTableCell">
                                        <xsl:value-of select="pa:intervenant/pa:prenom" /><xsl:text> </xsl:text><xsl:value-of select="pa:intervenant/pa:nom" />
                                    </td>
                                </tr>
                            </xsl:for-each>
                        </table>
                    </div>
                </main>

                <footer>

                </footer>
            </body>
        </html>
    </xsl:template>
    
    <xsl:template match="pa:acte">
       <li><xsl:value-of select="." /></li>
    </xsl:template>
</xsl:stylesheet>