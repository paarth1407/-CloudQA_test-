# Overview
A robust web form testing algorithm that uses multiple locator strategies to ensure test reliability despite HTML element changes.
# Algorithm Structure
1. Test Initialization Phase

ALGORITHM Initialize_Test_Environment()
BEGIN
    CREATE ChromeOptions with anti-detection settings
    SET browser arguments: disable-blink-features, no-sandbox
    INITIALIZE WebDriver with Chrome browser
    MAXIMIZE browser window
    CREATE WebDriverWait with 10-second timeout
END

2. Main Test Execution Flow

ALGORITHM Execute_Form_Tests()
BEGIN
    NAVIGATE to target URL
    WAIT 3 seconds for page load
    
    FOR each field_type IN [FirstName, Email, PhoneNumber]
        CALL Test_Field(field_type)
    END FOR
    
    OUTPUT "All tests completed successfully"
END

3. Individual Field Testing Algorithm

ALGORITHM Test_Field(field_type)
BEGIN
    OUTPUT "Testing {field_type} field..."
    
    element = CALL Find_Element_By_Type(field_type)
    
    IF element IS NOT NULL THEN
        CLEAR element content
        INPUT test_data INTO element
        retrieved_value = GET element.value attribute
        
        ASSERT retrieved_value EQUALS test_data
        OUTPUT "{field_type} field test passed"
    ELSE
        FAIL test WITH "Couldn't find the {field_type} field"
    END IF
END

4. Multi-Strategy Element Location Algorithm

ALGORITHM Find_Element_By_Type(field_type)
BEGIN
    locator_strategies = GET_STRATEGIES_FOR(field_type)
    
    FOR each strategy IN locator_strategies
        TRY
            element = WAIT_FOR_CLICKABLE_ELEMENT(strategy, 10_seconds)
            
            IF element.is_displayed AND element.is_enabled THEN
                SCROLL_INTO_VIEW(element)
                WAIT 500 milliseconds
                RETURN element
            END IF
            
        CATCH TimeoutException OR NoSuchElementException
            CONTINUE to next strategy
        END TRY
    END FOR
    
    RETURN NULL
END

5. Locator Strategy Definitions

First Name Field Strategies
STRATEGIES_FirstName = [
    By.Id("firstName"),
    By.Id("fname"),
    By.Name("firstName"),
    By.XPath("//input[@placeholder='First Name']"),
    By.XPath("//input[@placeholder='Name']"),
    By.CssSelector("input[placeholder*='First']"),
    By.CssSelector("input[placeholder*='Name']"),
    By.XPath("//input[contains(@class,'form-control')][@type='text'][1]")
]
Email Field Strategies
STRATEGIES_Email = [
    By.Id("userEmail"),
    By.Id("email"),
    By.Name("email"),
    By.XPath("//input[@type='email']"),
    By.XPath("//input[@placeholder='Email']"),
    By.CssSelector("input[type='email']"),
    By.CssSelector("input[placeholder*='email']"),
    By.XPath("//input[contains(@placeholder,'Email')]")
]
Phone Number Field Strategies
STRATEGIES_Phone = [
    By.Id("userNumber"),
    By.Id("mobile"),
    By.Id("phone"),
    By.Name("mobile"),
    By.XPath("//input[@type='tel']"),
    By.XPath("//input[@placeholder='Mobile Number']"),
    By.XPath("//input[contains(@placeholder,'Mobile')]"),
    By.XPath("//input[contains(@placeholder,'Phone')]"),
    By.CssSelector("input[placeholder*='Mobile']")
]

6. Cleanup Algorithm

ALGORITHM Cleanup_Resources()
BEGIN
    IF driver IS NOT NULL THEN
        QUIT driver
        RELEASE browser resources
    END IF
END
Key Algorithm Features
Resilience Mechanisms

Fallback Strategy Pattern: Multiple locator approaches per element
Exception Handling: Graceful degradation on locator failures
Element State Validation: Verify visibility and interactability
Dynamic Positioning: Automatic scrolling to element location

# Wait Strategy

Explicit Waits: WebDriverWait with expected conditions
Element Readiness: Wait for clickable state before interaction
Position Stabilization: Brief pause after scrolling

# Robustness Features

Cross-browser Compatibility: Chrome-specific optimizations
Anti-detection Measures: Disable automation control features
Resource Management: Proper driver cleanup in teardown

# Time Complexity

Best Case: O(1) - First locator strategy succeeds
Worst Case: O(n) - Where n is the number of locator strategies
Average Case: O(k) - Where k is the average position of successful locator

# Space Complexity 

O(1) - Constant space for driver instances and element references
O(n) - Linear space for storing locator strategy lists

# Fault Tolerance
The algorithm provides multiple levels of fault tolerance:-
Locator Level: Multiple strategies per element type
Element Level: State validation before interaction
Test Level: Individual test isolation
Session Level: Proper resource cleanup
