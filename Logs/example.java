import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import java.io.File;
import java.nio.file.Files;
import java.nio.file.Paths;

public class SoapModifier {

    public static void modifySoapXml(String filePath, String secretName) throws Exception {
        // Cargar el archivo XML
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        factory.setNamespaceAware(true);
        DocumentBuilder builder = factory.newDocumentBuilder();
        Document doc = builder.parse(new File(filePath));

        // Obtener el valor del Key Vault
        String sensitiveData = KeyVaultUtil.getSecret(secretName);

        // Modificar el mensaje SOAP
        Element identificacionElement = (Element) doc.getElementsByTagName("numeroIdentificacion").item(0);
        identificacionElement.setTextContent(sensitiveData);

        // Guardar el documento modificado
        TransformerFactory transformerFactory = TransformerFactory.newInstance();
        Transformer transformer = transformerFactory.newTransformer();
        DOMSource source = new DOMSource(doc);
        StreamResult result = new StreamResult(new File(filePath));
        transformer.transform(source, result);
    }
}
