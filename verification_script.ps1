$ErrorActionPreference = "Stop"
$baseUrl = "http://localhost:8080/api"
$testUsername = "verifUser_" + (Get-Date -Format "yyyyMMddHHmmss")
$testEmail = $testUsername + "@test.com"
$testPassword = "Password123!"

function Test-Endpoint {
    param($method, $url, $headers = @{}, $body = $null, $expectCode = 200)
    Write-Host "Testing $method $url..." -NoNewline
    try {
        $params = @{
            Method = $method
            Uri = $url
            Headers = $headers
            ContentType = "application/json"
        }
        if ($body) { $params.Body = ($body | ConvertTo-Json) }
        
        $response = Invoke-RestMethod @params -ErrorAction Stop -ResponseHeadersVariable headers
        
        Write-Host " [OK]" -ForegroundColor Green
        return $response
    } catch {
        $ex = $_
        if ($ex.Exception.Response.StatusCode.value__ -eq $expectCode) {
             Write-Host " [OK] (Expected Error)" -ForegroundColor Green
             return $null
        }
        Write-Host " [FAILED]" -ForegroundColor Red
        Write-Host "Error: $($ex.Message)"
        if ($ex.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($ex.Exception.Response.GetResponseStream())
            $body = $reader.ReadToEnd()
            Write-Host "Code: $($ex.Exception.Response.StatusCode.value__)" 
            Write-Host "Body: $body"
        } else {
             Write-Host "No response body."
        }
        return $null
    }
}

Write-Host "=== STARTING VERIFICATION ==="

# 1. Register
$registerBody = @{
    Username = $testUsername
    Email = $testEmail
    Password = $testPassword
    FirstName = "Verif"
    LastName = "User"
    DateOfBirth = "2000-01-01"
}
$regResult = Test-Endpoint -method POST -url "$baseUrl/Auth/register" -body $registerBody

# 2. Login
$loginBody = @{
    Email = $testEmail
    Password = $testPassword
}
$loginResult = Test-Endpoint -method POST -url "$baseUrl/Auth/login" -body $loginBody

if (-not $loginResult) {
    Write-Error "Login failed. Cannot proceed."
}

$token = $loginResult.token
$authHeader = @{ Authorization = "Bearer $token" }
Write-Host "Token obtained."

# 3. Themes (Public)
$themes = Test-Endpoint -method GET -url "$baseUrl/Theme"
if ($themes.Count -eq 0) { Write-Warning "No themes found." }
else { Write-Host "Found $($themes.Count) themes." }

# 4. Quiz (Public) - Croyance -> Tawheed -> Introduction
# Using IDs found previously
$themeId = "697b36a4330929e1e2acdd94"
$subjectId = "26aab077-ae69-44f3-b428-062153b4e969"
$partId = "b33ad090-45e1-4b31-a65d-55db01bfaa35"
$quizUrl = "$baseUrl/Theme/$themeId/subjects/$subjectId/parts/$partId/quiz"
$quiz = Test-Endpoint -method GET -url $quizUrl
if ($quiz) { Write-Host "Quiz loaded with $($quiz.Count) questions." }

# 5. User Profile (Auth)
$me = Test-Endpoint -method GET -url "$baseUrl/User/me" -headers $authHeader
if ($me) { Write-Host "Hello $($me.username)!" }

# 6. Dashboard Stats (Auth)
$stats = Test-Endpoint -method GET -url "$baseUrl/Stats/dashboard" -headers $authHeader
if ($stats) { Write-Host "Stats loaded. Global Score: $($stats.globalScore)" }

# 7. Progression (Auth)
$progression = Test-Endpoint -method GET -url "$baseUrl/Progression" -headers $authHeader
if ($progression) { Write-Host "Progression loaded." }

Write-Host "=== VERIFICATION COMPLETE ==="
