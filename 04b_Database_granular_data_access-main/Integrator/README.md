# Integrator

1. Establish a connection to the database using follow command:
```bash
psql -h 2.tcp.eu.ngrok.io -p 16179 -U sales_rep -d bookstore
```
2. Enter password when prompted. ```sales123```.

![Screenshot of being logged in](/04b_Database_granular_data_access-main/Integrator/screenshots/1.png "Title")

3. Then I tested the SQL queries from exposees documentation to validate which endpoints worked.
 
![Screenshot of being logged in](/04b_Database_granular_data_access-main/Integrator/screenshots/2.png "Title")

![Screenshot of being logged in](/04b_Database_granular_data_access-main/Integrator/screenshots/3.png "Title")

## Test Results

### What Worked ✅
- **Database Connection**: Successfully connected to the PostgreSQL database
- **SELECT Operations**: 
  - `SELECT * FROM books;` - Retrieved all book records (5 rows)
  - `SELECT * FROM customers_sales_view;` - Retrieved customer data from view (4 rows)
- **Data Retrieval**: Both queries returned expected data with proper formatting

### What Didn't Work ❌
- **UPDATE Operations**: 
  - `UPDATE books SET stock = 95 WHERE id = 1;` - **Permission denied**
  - `UPDATE books SET price = 13.99 WHERE id = 1;` - **Permission denied**
- **Credit Card Access**: 
  - `SELECT credit_card FROM customers;` - **Permission denied**

### Permission Analysis
The database implements **granular access control** with fine-grained permissions:
- ❌ **Table-level write restrictions**: UPDATE operations on books table are blocked
- ❌ **Column-level read restrictions**: Direct access to `credit_card` column is denied
- ✅ **Table-level read permissions**: Full SELECT access to books table
- ✅ **View-based data access**: Filtered customer data through `customers_sales_view`

### Granular Access Control Features Observed
1. **Column-Level Security**: Sensitive data (credit cards) hidden from direct queries
2. **Operation-Level Restrictions**: Read operations allowed, write operations blocked
3. **View-Based Filtering**: Customer data accessible only through controlled view
4. **Table-Specific Permissions**: Different access levels per table/view

## Conclusion
The database demonstrates **granular access control** implementation where permissions are defined at multiple levels (table, column, operation, and view). This provides fine-grained security that goes beyond simple role assignments, allowing precise control over what data can be accessed and how it can be manipulated by different users.